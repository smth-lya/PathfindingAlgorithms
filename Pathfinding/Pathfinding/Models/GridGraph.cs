

namespace Pathfinding.Models
{
    public class GridGraph : IGraph<(int x, int y)>
    {
        private float[,] costs;

        private int height;
        private int width;
        
        public GridGraph(float[,] costs)
        {
            this.costs = costs;
            this.width = costs.GetLength(0);
            this.height = costs.GetLength(1);
        }

        public float GetCost((int x, int y) from, (int x, int y) to)
        {
            return costs[to.y, to.x];
        }


        public IEnumerable<(int x, int y)> GetNeighbours((int x, int y) node)
        {
            int[] dX = { 0, 0, -1, 1 };
            int[] dY = { -1, 1, 0, 0 };

            foreach (var (dx, dy) in dX.Zip(dY, (x, y) => (x, y)))
            {
                int x = node.x + dx;
                int y = node.y + dy;

                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    yield return (x, y);
                }
            }
        }

        public bool IsWall((int x, int y) node)
        {
            return costs[node.y, node.x] == float.PositiveInfinity;
        }
    }
}
