using Pathfinding.Models;

namespace Pathfinding.Pathfinders
{
    public static class BFS
    {
        public static List<T>? FindPath<T>(IGraph<T> graph, T start, T end) where T : notnull
        {
            Queue<T> queue = new Queue<T>();
            queue.Enqueue(start);

            HashSet<T> visited = new HashSet<T>();
            visited.Add(start);

            Dictionary<T, float> distances = new Dictionary<T, float>();
            distances.Add(start, 0);

            while (queue.Count >= 0)
            {
                var current = queue.Dequeue();

                foreach (var neighbour in graph.GetNeighbours(current))
                {
                    if (graph.IsWall(neighbour)) continue;
                   
                    if (!visited.Contains(neighbour))
                    {
                        distances[neighbour] = Math.Min(distances[current] + 1, distances[neighbour]);

                        queue.Enqueue(neighbour);
                        visited.Add(neighbour);
                    }
                }
            }

            if (!visited.Contains(end))
                return null;

            return RestorePath(graph, distances, end).ToList();
        }
        private static IEnumerable<T> RestorePath<T>(IGraph<T> graph, Dictionary<T, float> distances, T end) where T : notnull
        {
            var current = end;
            var cost = distances[end];

            while (true)
            {
                foreach (var neighbour in graph.GetNeighbours(current))
                {
                    if (distances[neighbour] + 1 == cost)
                    {
                        current = neighbour;
                        cost--;
                        yield return current;
                        break;
                    }
                }   
            }
        }
    }
}
