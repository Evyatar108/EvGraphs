using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace EvImps.Graphs.Algorithms.Search
{
	public class BreadthFirstSearch : ISearchAlgorithm
	{
		public void Run(IGraph g, string sId) =>
			Search(g, sId, (v) => false);


		public IVerticle Search(IGraph g,string sId,
		                               Predicate<IVerticle> predicate)
		{
			g.ResetStats();
			return HelperSearch(g,sId,predicate);
		}

		private IVerticle HelperSearch(IGraph g, string sId, 
		                               Predicate<IVerticle> predicate)
		{
			Contract.Requires(sId != null);
			Contract.Requires(g != null);
			Contract.Requires(predicate != null);
			Contract.Ensures(predicate(Contract.Result<Verticle>()));
			Contract.Ensures(g.ContainsVerticle(Contract.Result<Verticle>()));
			var queue = new Queue<IVerticle>();
			IVerticle v = g.GetVerticle(sId);
			v.Dist = 0;
			v.IsVisited = true;
			if (predicate(v))
				return v;
			queue.Enqueue(v);
			IVerticle u;
			while (queue.Count > 0)
			{
				v = queue.Dequeue();
				foreach (Edge e in v.EdgesOut)
				{
					u = e.To;
					if (!u.IsVisited)
					{
						if (predicate(u))
							return u;
						u.IsVisited = true;
						u.Dist = v.Dist + 1;
						u.ParentEdge = e;
						queue.Enqueue(u);
					}
				}
			}
			return null;
		}

		public IVerticle FullSearch(IGraph g,
		                                   Predicate<IVerticle> predicate)
		{
			g.ResetStats();
			IVerticle result;
			foreach(IVerticle v in g)
				if(!v.IsVisited 
				   && (result=HelperSearch(g,v.Id,predicate)) != null)
					return result;
			return null;
		}
	}
}
