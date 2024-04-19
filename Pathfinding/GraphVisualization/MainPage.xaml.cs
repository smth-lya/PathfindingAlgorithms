using GraphVisualization.GraphsUI;
using Pathfinding.Models;
using System.Diagnostics.CodeAnalysis;

namespace GraphVisualization
{
    public partial class MainPage : ContentPage
    {
        private GridGraphUI _graphUI;

        public MainPage()
        {
            InitializeComponent();
            InitializeGridGraph();
            InitializeBrushButtons();
        }

        [MemberNotNull(nameof(_graphUI))]
        private void InitializeGridGraph()
        {
            GridGraph graph = new GridGraph(new float[20, 20]);
            GridGraphUI graphUI = new GridGraphUI(graph);
            
            _graphUI = graphUI;

            Layout.Add(graphUI);
        }
        private void InitializeBrushButtons()
        {
            EndBrushButton.Clicked += (s, e) => { _graphUI.SwitchBrush(GridGraphUI.Brush.End); };
            WallBrushButton.Clicked += (s, e) => { _graphUI.SwitchBrush(GridGraphUI.Brush.Wall); };
            StartBrushButton.Clicked += (s, e) => { _graphUI.SwitchBrush(GridGraphUI.Brush.Start); };

            RedrawButton.Clicked += (s, e) => { _graphUI.FindPath(); };
            ClearButton.Clicked += (s, e) => { _graphUI.BackAllToDefaultColor(); };

            RandomButton.Clicked += (s, e) => { _graphUI.GenerateRandomGrid(); };
        }

        private void OnButtonPressedAnimation(object? sender, EventArgs e)
        {
            Button? button = sender as Button;
            
            if (button == null) return;

            var animation = new Animation((value) =>
            {
                button.Opacity = value;
            }, 1, 0.5);

            button.Animate("Opacity", animation, length: 100);
        }
        private void OnButtonReleasedAnimation(object? sender, EventArgs e)
        {
            Button? button = sender as Button;

            if (button == null) return;

            var animation = new Animation((value) =>
            {
                button.Opacity = value;
            }, 0.5, 1);

            button.Animate("Opacity", animation, length: 100);
        }
    }

}
