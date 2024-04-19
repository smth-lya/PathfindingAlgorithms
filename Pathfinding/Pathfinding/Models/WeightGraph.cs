namespace Pathfinding.Models
{
    internal class WeightGraph<T> where T : Node
    {
        private Dictionary<T, Dictionary<T, float>> _adj;

        public WeightGraph() 
        {
            _adj = new Dictionary<T, Dictionary<T, float>>();
        }
        public WeightGraph(Dictionary<T, Dictionary<T, float>> adj)
        {
            _adj = adj.ToDictionary();
        }

        public void AddNode(T node)
        {
            _adj[node] = new Dictionary<T, float>();
        }
        public void AddEdge(T from, T to, float coast)
        {
            if (!_adj.ContainsKey(from))
            {
                AddNode(from);
            }
            
            if (!_adj.ContainsKey(to))
            {
                AddNode(to);
            }

            _adj[from][to] = coast;
        }
        public float GetCost(T from, T to)
        {
            if (!_adj.ContainsKey(from) || !_adj.ContainsKey(to))
            {
                return float.PositiveInfinity;
            }

            return _adj[from][to];
        }

        public IEnumerable<T> GetNeighbours(T node)
        {
            if (_adj.ContainsKey(node))
            {
                foreach (var neighbour in _adj[node].Keys)
                {
                    yield return neighbour;
                }
            }
        }
    }
}
