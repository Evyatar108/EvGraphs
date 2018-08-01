using System.Collections.Generic;
using SortedStructures;
using System.Diagnostics.Contracts;

namespace EvImps.Graphs
{
	public class Graph<TId,TData> : IGraph<TId,TData>
	{
		public bool IsDirected {get;}
		private Dictionary<TId, IVerticle<TId,TData>> verticles;
		public IEnumerable<IVerticle<TId,TData>> Verticles { get{ return verticles.Values;}}
		public IEnumerable<IEdge<TId,TData>> Edges 
		{
			get 
			{ 
				foreach(var v in verticles.Values)
					foreach(var e in v.EdgesOut)
						yield return e;
			}
		}

		public int NumOfEdges{get; private set;}

		int numOfVerticles;
		public int NumOfVerticles { get { return numOfVerticles; } }
        
		public Graph(bool isDirected=true)
		{
			verticles = new Dictionary<TId, IVerticle<TId,TData>>();
			numOfVerticles = 0;
			IsDirected = isDirected;
		}

		public bool ContainsVerticle(IVerticle<TId,TData> v)=>
			ContainsVerticle(v.Id);

		public bool ContainsVerticle(TId vId){
		    return verticles.ContainsKey(vId);
		}
        
		public bool AddVerticle(IVerticle<TId,TData> v)=>
			AddVerticle(v.Id);
        
		public bool AddVerticle(TId vId)
		{
			Contract.Ensures(ContainsVerticle(vId));
			if (ContainsVerticle(vId))
				return false;
			numOfVerticles++;
			verticles[vId] = new Verticle<TId,TData>(vId);
			return true;
		}

		public bool RemoveVerticle(TId vId)
		{
			Contract.Ensures(!ContainsVerticle(vId));
			if(!verticles.TryGetValue(vId,out IVerticle<TId,TData> v))
				return false;
			var edgesToRemove = new List<IEdge<TId,TData>>();
			foreach(var e in v.Edges)
				edgesToRemove.Add(e);
			foreach(var e in edgesToRemove)
				RemoveEdge(e);
			verticles.Remove(vId);
			return true;
		}
        
		public bool ContainsEdge(IEdge<TId,TData> e)=> ContainsEdge(e.From.Id,e.To.Id);
        
		public bool ContainsEdge(TId vId,TId uId){
			return ContainsVerticle(vId) && ContainsVerticle(uId) 
				&& GetVerticle(vId).IsNeighborOut(uId);
		}

		public bool ContainsRevEdge(IEdge<TId,TData> e)=>
		    ContainsEdge(e.To.Id,e.From.Id);

		public bool ContainsRevEdge(TId vId,TId uId)=>
        ContainsEdge(uId,vId);
        
		public bool AddEdge(IEdge<TId,TData> e)=>
			AddEdge(e.From.Id,e.To.Id, e.Weight);

		public bool AddEdge(TId vId, TId uId, int w=1)
        {
			Contract.Ensures(ContainsEdge(vId,uId));
			if(ContainsEdge(vId,uId))
				return false;
			AddVerticle(vId);
            AddVerticle(uId);
			IVerticle<TId,TData> v = GetVerticle(vId);
			IVerticle<TId,TData> u = GetVerticle(uId);
			IEdge<TId,TData> e = new Edge<TId,TData>(v,u, w);
            v.AddOutNeighbor(e);
            u.AddInNeighbor(e);
			if(!IsDirected)
			{
				e = new Edge<TId,TData>(u,v, w);
				v.AddInNeighbor(e);
                u.AddOutNeighbor(e);
			}
			NumOfEdges++;
			return true;
        }

		public IEdge<TId,TData> GetEdge(TId vId,TId uId){
			Contract.Requires(ContainsEdge(vId,uId));
			return GetVerticle(vId).GetEdgeOut(uId);
		}

		public bool RemoveEdge(IEdge<TId,TData> e)=>
		    RemoveEdge(e.From.Id,e.To.Id);

		public bool RemoveEdge(TId vId, TId uId)
		{
			Contract.Ensures(!ContainsEdge(vId,uId));
			if(!ContainsEdge(vId,uId))
				return false;
			var v = GetVerticle(vId);
			var u = GetVerticle(uId);
			v.RemoveEdgeOut(uId);
			u.RemoveEdgeIn(vId);
			if(!IsDirected)
			{
				v.RemoveEdgeIn(uId);
                u.RemoveEdgeOut(vId);
			}
			return true;
		}

		public IVerticle<TId,TData> GetVerticle(TId vId)
		{
			Contract.Requires(ContainsVerticle(vId));
			return verticles[vId];
		}

		public void ResetStats()
		{
			foreach (var v in verticles.Values)
				v.ResetStats();
		}
           
		//public bool IsConnected()
		//{
		//	if(NumOfVerticles==0)
		//		return true;
		//	Dfs(verticles.First().Value.Id);
		//	foreach(Verticle v in Verticles)
		//		if(!v.IsVisited)
		//			return false;
		//	return true;
		//}

		private IEdge<TId,TData> GetMinUnvisitedEdge(Heap<IEdge<TId,TData>> edges){
			bool found = false;
			IEdge<TId,TData> e=null;
			while(!found)
			{
				e = edges.ExtractMin();
				found = !e.To.IsVisited;
			}
			return e;
		}

		//public void Dijkstra(string sId) // Complexity: O(V^2)
		//{
		//	Contract.Requires(sId!=null);
		//	Contract.Requires(ContainsVerticle(sId));
		//	ResetStats();
		//	IVerticle<TId,TData> v = verticles[sId];
  //          v.IsVisited = true;
		//	v.Dist=0;
		//	RelaxNeighbores(v);
		//	for(int i=1;i<numOfVerticles;i++)
  //          {
		//		v = FindMinUnvisitedVerticle();
  //              v.IsVisited=true;
		//		RelaxNeighbores(v); 
  //          }
		//}

		private static void RelaxNeighbores(IVerticle<TId,TData> v)
		{
			foreach(var e in v.EdgesOut)
				Relax(e);
		}

		private static void Relax(IEdge<TId,TData> e)
		{
			IVerticle<TId,TData> v = e.From;
			IVerticle<TId,TData> u = e.To;
			if(u.Dist > v.Dist + e.Weight){
				u.Dist = v.Dist + e.Weight;
				u.ParentEdge = e;
			}
		}

		private IVerticle<TId,TData> FindMinUnvisitedVerticle(){
			IVerticle<TId,TData> res = null;
			foreach(var v in verticles.Values)
				if(!v.IsVisited && (res==null || res.Dist > v.Dist) )
					res = v;
            return res;
        }

		//public bool HasNegativeCycle()
		//{
		//	BellmanFord(Verticles.First().Id);
		//	foreach(Edge e in Edges)
		//		if(e.To.Dist > e.From.Dist+e.Weight)
		//			return true;
		//	return false;
		//}

		//public IList<IEdge<TId,TData>> GetShortestPath(TId sId,TId tId,bool statsReady=false)
		//{
		//	Contract.Requires(ContainsVerticle(sId));
		//	Contract.Requires(ContainsVerticle(tId));
		//	if(!statsReady)
		//		Bfs(sId);
		//	IVerticle<TId,TData> s = GetVerticle(sId);
		//	IVerticle<TId,TData> v = GetVerticle(tId);
		//	List<IEdge<TId,TData>> path = new List<IEdge<TId,TData>>();
		//	while(v!=s)
		//	{
		//		path.Add(v.ParentEdge);
		//		v= v.ParentEdge.From;
		//	}
		//	path.Reverse();
		//	return path;
		//}
        

		//public bool IsTree()
		//{
		//	if(IsDirected)
  //              throw new NotSupportedException("No support for directed graphs");
		//	if(NumOfEdges != 2*( NumOfVerticles-1))
		//		return false;
		//	return IsConnected();
		//}

		public IGraph<TId,TData> GetSubGraph(IEnumerable<IEdge<TId,TData>> edges )
		{
			Contract.Requires(edges!=null);
			var g = new Graph<TId,TData>(IsDirected);
			foreach(var e in edges)
				g.AddEdge(e);
			return g;
		}

		public IGraph<TId,TData> GetSubGraph(IEnumerable<IVerticle<TId,TData>> verts)
        {
			Contract.Requires(verts!=null);
			var g = new Graph<TId,TData>(IsDirected);
			foreach(var v in verts)
				g.AddVerticle(v);
			foreach(var v in verts)
				foreach(var e in v.EdgesOut)
					if(g.ContainsVerticle(e.To.Id))
					    g.AddEdge(e);
			return g;
        }
	}
}


