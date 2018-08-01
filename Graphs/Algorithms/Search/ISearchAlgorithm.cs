using System;
namespace EvImps.Graphs.Algorithms.Search
{
	public interface ISearchAlgorithm
    {
		void Run(IGraph g, string sId);
		IVerticle Search(IGraph g, string sId, 
		                 Predicate<IVerticle> predicate);
		IVerticle FullSearch(IGraph g,
                             Predicate<IVerticle> predicate)
    }
}
