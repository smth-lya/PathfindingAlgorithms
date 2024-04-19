using Pathfinding.Models;
using Pathfinding.Pathfinders;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace GraphVisualization.GraphsUI
{
    public class GridGraphUI : Grid
    {
        private readonly GridGraph _graph;

        private readonly Vector2 cellSize = new Vector2(44, 44);

        private Dictionary<CellUI, (int x, int y)> _positions;
        private Dictionary<(int x, int y), CellUI> _cells;

        public enum Brush { Empty, Start, End, Path, Wall }
        private Brush _brush;

        private CellUI? _currentStart;
        private CellUI? _currentEnd;

        public GridGraphUI(GridGraph graph)
        {
            ArgumentNullException.ThrowIfNull(graph, nameof(graph));

            _graph = graph;

            InitializeGraph();
        }

        [MemberNotNull(nameof(_positions), nameof(_cells))]
        private void InitializeGraph()
        {
            MaximumHeightRequest = 700;
            MinimumHeightRequest = 700;
            
            MaximumWidthRequest = 700;
            MinimumWidthRequest = 700;
            
            RowSpacing = 10d / _graph.Height * 5;
            ColumnSpacing = 10d / _graph.Width * 5;

            Clear();

            for (int y = 0; y < _graph.Height; y++)
            {
                RowDefinitions.Add(new RowDefinition());
            }

            for (int x = 0; x < _graph.Width; x++)
            {
                ColumnDefinitions.Add(new ColumnDefinition());
            }

            _positions = new ();
            _cells = new ();

            for (int y = 0; y < _graph.Height; y++)
            {
                for (int x = 0; x < _graph.Width; x++)
                {
                    bool isWall = _graph.IsWall((x, y));

                    var cell = CreateNewCell(isWall);
                   
                    InitializeCell(cell);

                    this.Add(cell, x, y);
                    _positions.Add(cell, (x, y));
                    _cells.Add((x, y), cell);
                }
            }
        }

        private CellUI CreateNewCell(bool isWall)
        {
            Color color = GetColorBrush(isWall ? Brush.Wall : Brush.Empty);

            return new CellUI(color)
            {
                MaximumHeightRequest = cellSize.Y,
                MaximumWidthRequest = cellSize.X,

                MinimumHeightRequest = cellSize.Y,
                MinimumWidthRequest = cellSize.X,

                CornerRadius = 0,
                BorderWidth = 0,

                BackgroundColor = color,
            };
        }
        private void InitializeCell(in CellUI cell)
        {
            cell.Clicked += OnCellClicked;

            cell.Pressed += OnCellPressedAnimation;
            cell.Pressed += OnCellReleasedAnimation;
        }

        private void OnCellClicked(object? sender, EventArgs e)
        {
            CellUI? CellUI = sender as CellUI;

            if (CellUI == null) return;

            ChangeCellColor(CellUI, _brush);
        }

        private void OnCellPressedAnimation(object? sender, EventArgs e)
        {
            CellUI? CellUI = sender as CellUI;

            if (CellUI == null) return;

            var animation = new Animation((value) =>
            {
                CellUI.Opacity = value;
            }, 1, 0.5);

            CellUI.Animate("Opacity", animation, length: 100);
        }
        private void OnCellReleasedAnimation(object? sender, EventArgs e)
        {
            CellUI? CellUI = sender as CellUI;

            if (CellUI == null) return;

            var animation = new Animation((value) =>
            {
                CellUI.Opacity = value;
            }, 0.5, 1);

            CellUI.Animate("Opacity", animation, length: 100);
        }


        public void SwitchBrush(Brush brush)
        {
            _brush = brush;  
        }
        private Color GetColorBrush(Brush brush)
        {
            return (brush) switch
            {
                Brush.Empty => Colors.WhiteSmoke,
                Brush.Wall => Colors.DarkGray,
                Brush.Start => Colors.LimeGreen,
                Brush.End => Colors.Crimson,
                Brush.Path => Colors.YellowGreen,
                _ => Colors.White,
            };
        }
        private void ChangeCellColor(CellUI cell, Brush brush)
        {
            Color color = GetColorBrush(brush);

            if (brush != Brush.Wall && cell.BackgroundColor == GetColorBrush(Brush.Wall))
            {
                return;
            }

            switch (brush)
            {
                case Brush.Empty:
                    cell.BackgroundColor = color;
                    break;
                case Brush.Start:

                    if (_currentStart != null)
                    {
                        _currentStart.BackgroundColor = _currentStart.DefaultColor;
                    }

                    _currentStart = cell;

                    _currentStart.BackgroundColor = color;

                    break;
                case Brush.End:

                    if (_currentEnd != null)
                    {
                        _currentEnd.BackgroundColor = _currentEnd.DefaultColor;
                    }
                    
                    _currentEnd = cell;

                    _currentEnd.BackgroundColor = color;

                    break;
                case Brush.Path:
                    if (cell.BackgroundColor == color)
                    {
                        cell.BackgroundColor = cell.DefaultColor;
                    }

                    cell.BackgroundColor = color;

                    break;
                case Brush.Wall:

                    if (cell.BackgroundColor == GetColorBrush(Brush.Start))
                    {
                        _currentStart = null;
                    }

                    if (cell.BackgroundColor == GetColorBrush(Brush.End))
                    {
                        _currentEnd = null;
                    }

                    if (cell.BackgroundColor == color)
                    {
                        cell.BackgroundColor = cell.DefaultColor;
                        _graph.ChangeCost(_positions[cell], 1);
                    }
                    else
                    {
                        _graph.ChangeCost(_positions[cell], float.PositiveInfinity);
                        cell.BackgroundColor = color;
                    }
                    break;
                default:
                    break;
            }
        }


        public void FindPath()
        {
            if (_currentStart == null || _currentEnd == null) return;

            if (!_positions.ContainsKey(_currentStart) || !_positions.ContainsKey(_currentEnd)) return;

            var path = BFS.FindPath(_graph, _positions[_currentStart], _positions[_currentEnd]);

            ClearPath();

            for (int i = 1; i < path.Count - 1; i++)
            {
                var cell = _cells[path[i]];

                ChangeCellColor(cell, Brush.Path);
            }
        }

        private void ClearPath()
        {
            Color pathColor = GetColorBrush(Brush.Path);

            foreach (var cell in _cells.Values)
            {
                if (cell.BackgroundColor == pathColor)
                {
                    cell.BackgroundColor = cell.DefaultColor;
                }
            }
        }

        public void BackAllToDefaultColor()
        {
            foreach (var cell in _cells.Values)
            {
                cell.BackgroundColor = cell.DefaultColor;
            }
        }

        public void GenerateRandomGrid()
        {
            Random random = new Random();

            for (int y = 0; y < _graph.Height; y++)
            {
                for (int x = 0; x < _graph.Width; x++)
                {
                    if (random.NextDouble() < 0.5)
                    {
                        _graph.ChangeCost((x, y), float.PositiveInfinity);
                        _cells[(x, y)].BackgroundColor = GetColorBrush(Brush.Wall);
                    }
                    else
                    {
                        _graph.ChangeCost((x, y), 1);
                        _cells[(x, y)].BackgroundColor = GetColorBrush(Brush.Empty);
                    }
                }
            }
        }
    }
}
