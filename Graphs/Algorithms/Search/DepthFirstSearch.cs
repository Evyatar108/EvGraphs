using System;
using System.Diagnostics.Contracts;

namespace EvImps.Graphs.Algorithms.Search
{
	public class DepthFirstSearch : ISearchAlgorithm
	{
		public void Run(IGraph g, string sId)=>
		Search(g,sId,(v)=>false);

		public IVerticle Search(IGraph g, string sId, 
                      Predicate<IVerticle> predicate)
		{
			g.ResetStats();
			return SearchHelper(g,sId,predicate);
		}

		private IVerticle SearchHelper(IGraph g, string sId, 
		                     Predicate<IVerticle> predicate)
		{
			Contract.Requires(sId!=null);
            Contract.Requires(g.ContainsVerticle(sId));
            g.ResetStats();
            IVerticle s = g.GetVerticle(sId);
            s.Dist=0;
			return Search(g,s,predicate);
		}
        
		private IVerticle SearchHelper(IGraph g, IVerticle v,
		                                Predicate<IVerticle> predicate)
		{
			v.IsVisited = true;
            IVerticle u;
            foreach (Edge e in v.EdgesOut)
            {
                u = e.To;
                if (!u.IsVisited)
                {
                    u.ParentEdge = e;
                    u.Dist = v.Dist+1;
					Search(g,u,predicate);
                }
            }
			return null;
		}
        
		public IVerticle FullSearch(IGraph g,
                                           Predicate<IVerticle> predicate)
        {
            IVerticle result;
            foreach(IVerticle v in g)
				if(!v.IsVisited && (result=SearchHelper(g,v.Id,predicate)) != null)
                    return result;
            return null;
        }
	}
}
