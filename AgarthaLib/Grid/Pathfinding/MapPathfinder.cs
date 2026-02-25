namespace AgarthaLib.Grid.Pathfinding
{
    public class MapPathfinder<T>
    {
        public delegate float CostFunction(T a, T b);
        public CostFunction HeuristicCost, NodeTraversalCost;


    }
}
