using System;
namespace EvImps.Graphs.Algorithms.MinimumSpanningTree
{
    public interface IMinimumSpanningTreeAlgorithm
    {
		IGraph FindMinimumSpanningTree(IGraph g);
    }
}
