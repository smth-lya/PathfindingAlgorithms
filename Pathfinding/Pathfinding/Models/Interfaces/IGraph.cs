namespace Pathfinding.Models
{
    public interface IGraph<T>
    {
        IEnumerable<T> GetNeighbours(T node);
        float GetCost(T from, T to);
        bool IsWall(T node);
    }
}
