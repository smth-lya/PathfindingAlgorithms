namespace Pathfinding.Models
{
    public class GridGraph : IGraph<(int x, int y)>
    {
        private float[,] _costs;

        private int _height;
        private int _width;
        
        public int Height => _height;
        public int Width => _width;

        public GridGraph(float[,] costs)
        {
            this._costs = (float[,])costs.Clone();
            this._height = costs.GetLength(0);
            this._width = costs.GetLength(1);
        }
        public float GetCost((int x, int y) from, (int x, int y) to)
        {
            return _costs[to.y, to.x];
        }

        public IEnumerable<(int x, int y)> GetNeighbours((int x, int y) node)
        {
            int[] dX = { 0, 0, -1, 1 };
            int[] dY = { -1, 1, 0, 0 };

            foreach (var (dx, dy) in dX.Zip(dY, (x, y) => (x, y)))
            {
                int x = node.x + dx;
                int y = node.y + dy;

                if (x >= 0 && x < _width && y >= 0 && y < _height)
                {
                    yield return (x, y);
                }
            }
        }
        public void ChangeCost((int x, int y) position, float cost)
        {
            int x = position.x;
            int y = position.y;

            if (x < 0 || x >= Width || y < 0 || y >= Height) return;

            _costs[y, x] = cost;
        }
        public bool IsWall((int x, int y) node)
        {
            return _costs[node.y, node.x] == float.PositiveInfinity;
        }
    }
}
