using System;
using System.Linq;
using SortedStructures;

namespace EvImps.Graphs.Algorithms.MinimumSpanningTree
{
	public class Prim : IMinimumSpanningTreeAlgorithm
    {
		public IGraph FindMinimumSpanningTree(IGraph g){
			if(!g.IsConnected())
                throw new InvalidOperationException("Graph is not connected");
            g.ResetStats();
			IVerticle v = g.Verticles.First();
            v.IsVisited = true;
            IEdge[] treeEdges = new IEdge[g.NumOfVerticles-1];
            int teIndex= 0;
            var edges = new Heap<IEdge>(v.EdgesOut);
            IEdge e;
            while (teIndex<g.NumOfVerticles-1)
            {
                e = GetMinUnvisitedEdge(edges);
                treeEdges[teIndex++] = e;
                v = e.To;
                v.IsVisited=true;
                edges.AddRange(v.EdgesOut);
            }
            return g.GetSubGraph(treeEdges);
		}

		private IEdge GetMinUnvisitedEdge(Heap<IEdge> edges){
            bool found = false;
            IEdge e=null;
            while(!found)
            {
                e = edges.ExtractMin();
                found = !e.To.IsVisited;
            }
            return e;
        }
    }
}
