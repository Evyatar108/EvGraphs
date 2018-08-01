using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace EvImps.Graphs
{
	public class Graph<TVerticle,TData> : IGraph<TVerticle,TData>
	{
		private Dictionary<TVerticle, VerticleHolder> verticles;
		public bool IsDirected {get;}
		public IEnumerable<TVerticle> Verticles { get{ return verticles.Keys;}}
		public IEnumerable<IEdge<TVerticle>> Edges 
		{
			get 
			{ 
				foreach(var v in verticles.Keys)
					foreach(var e in GetEdgesOut(v))
						yield return e;
			}
		}

		public int NumOfEdges{get; private set;}
		public int NumOfVerticles { get; private set; }
        
		public Graph(bool isDirected=true)
        {
            NumOfVerticles = 0;
			NumOfEdges = 0;
            IsDirected = isDirected;
			verticles = new Dictionary<TVerticle, VerticleHolder>();
        }

		public IEnumerable<IEdge<TVerticle>> GetEdgesOut(TVerticle v)=>
		verticles[v].EdgesOut.Values;

		public IEnumerable<IEdge<TVerticle>> GetEdgesIn(TVerticle v)=>
        verticles[v].EdgesIn.Values;

		public IEnumerable<IEdge<TVerticle>> GetEdges(TVerticle v){
			VerticleHolder vHolder = verticles[v];
			return vHolder.EdgesIn.Values.Concat(vHolder.EdgesOut.Values);
        }
        
		public TData GetData(TVerticle v)=>
		verticles[v].Data;

		public void SetData(TVerticle v, TData data)=>
		verticles[v].Data = data;

		public bool ContainsVerticle(TVerticle v){
		    return verticles.ContainsKey(v);
		}

		public bool TryAddVerticle(TVerticle v, TData data)
		{
			if(ContainsVerticle(v))
				return false;
			AddVerticle(v,data);
			return true;
		}
        
		public void AddVerticle(TVerticle v,TData data = default(TData))
		{
			Contract.Requires(!ContainsVerticle(v));
			Contract.Ensures(ContainsVerticle(v));
			NumOfVerticles++;
			verticles[v] = new VerticleHolder(data);
		}

		public bool TryRemoveVerticle(TVerticle v)
        {
			if(!ContainsVerticle(v))
                return false;
			RemoveVerticle(v);
			return true;
        }

		public void RemoveVerticle(TVerticle v)
		{
			Contract.Requires(ContainsVerticle(v));
			Contract.Ensures(!ContainsVerticle(v));
			var edgesToRemove = new List<IEdge<TVerticle>>();
			foreach(var e in GetEdges(v))
				edgesToRemove.Add(e);
			foreach(var e in edgesToRemove)
				RemoveEdge(e);
			verticles.Remove(v);
		}
        
		public bool ContainsEdge(IEdge<TVerticle> e)=> ContainsEdge(e.From,e.To);
        
		public bool ContainsEdge(TVerticle v,TVerticle u){
			return ContainsVerticle(v) && ContainsVerticle(u) 
				&& verticles[v].EdgesOut.ContainsKey(u);
		}

		public bool ContainsReverseEdge(IEdge<TVerticle> e)=>
		    ContainsEdge(e.To,e.From);

		public bool ContainsReverseEdge(TVerticle v,TVerticle u)=>
        ContainsEdge(u,v);

		public bool TryAddEdge(IEdge<TVerticle> e)=>
		TryAddEdge(e.From,e.To);

		public bool TryAddEdge(TVerticle v, TVerticle u, int w=1){
                if(!ContainsVerticle(v) || !ContainsVerticle(u) || ContainsEdge(v,u))
                    return false;
                AddEdge(v,u, w);
                return true;
		}
        
		public void AddEdge(IEdge<TVerticle> e)=>
		AddEdge(e.From,e.To, e.Weight);

		public void AddEdge(TVerticle v, TVerticle u, int w=1)
        {
			Contract.Requires(!ContainsEdge(v,u));
			Contract.Requires(ContainsVerticle(v));
			Contract.Requires(ContainsVerticle(u));
			Contract.Ensures(ContainsEdge(v,u));
			IEdge<TVerticle> e = new Edge<TVerticle>(v,u, w);
            AddOutEdge(v,e);
			AddInEdge(u,e);
			if(!IsDirected)
			{
				e = new Edge<TVerticle>(u,v, w);
				AddOutEdge(u,e);
                AddInEdge(v,e);
			}
			NumOfEdges++;
        }

		public IEdge<TVerticle> GetEdge(TVerticle v,TVerticle u){
			Contract.Requires(ContainsEdge(v,u));
			return verticles[v].EdgesOut[u];
		}

		public bool TryRemoveEdge(IEdge<TVerticle> e)=>
		TryRemoveEdge(e.From,e.To);

		public bool TryRemoveEdge(TVerticle v,TVerticle u)
		{
			if(!ContainsEdge(v,u))
				return false;
			RemoveEdge(v,u);
			return true;
		}

		public void RemoveEdge(IEdge<TVerticle> e)=>
		    RemoveEdge(e.From,e.To);

		public void RemoveEdge(TVerticle v, TVerticle u)
		{
			Contract.Requires(ContainsEdge(v,u));
			Contract.Ensures(!ContainsEdge(v,u));
			RemoveEdgeOut(v,u);
			RemoveEdgeIn(u,v);
			if(!IsDirected)
			{
				RemoveEdgeIn(u,v);
                RemoveEdgeOut(v,u);
			}
		}

		private void AddOutEdge(TVerticle v,IEdge<TVerticle> e)=>
			verticles[v].EdgesOut[e.To] = e;

		private void AddInEdge(TVerticle v,IEdge<TVerticle> e)=>
            verticles[v].EdgesIn[e.From] = e;

		private void RemoveEdgeOut(TVerticle v,TVerticle u)=>
		verticles[v].EdgesOut.Remove(u);

		private void RemoveEdgeIn(TVerticle v,TVerticle u)=>
		verticles[u].EdgesIn.Remove(v);

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

		//private IEdge<TVerticle> GetMinUnvisitedEdge(Heap<IEdge<TVerticle>> edges){
		//	bool found = false;
		//	IEdge<TVerticle> e=null;
		//	while(!found)
		//	{
		//		e = edges.ExtractMin();
		//		found = !e.To.IsVisited;
		//	}
		//	return e;
		//}

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

		//private static void RelaxNeighbores(IVerticle<TVerticle,TData> v)
		//{
		//	foreach(var e in v.EdgesOut)
		//		Relax(e);
		//}

		//private static void Relax(IEdge<TVerticle,TData> e)
		//{
		//	IVerticle<TVerticle,TData> v = e.From;
		//	IVerticle<TVerticle,TData> u = e.To;
		//	if(u.Dist > v.Dist + e.Weight){
		//		u.Dist = v.Dist + e.Weight;
		//		u.ParentEdge = e;
		//	}
		//}

		//private IVerticle<TVerticle,TData> FindMinUnvisitedVerticle(){
			//IVerticle<TVerticle,TData> res = null;
			//foreach(var v in verticles.Values)
				//if(!v.IsVisited && (res==null || res.Dist > v.Dist) )
					//res = v;
        //    return res;
        //}

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

		public IGraph<TVerticle,TData> GetSubGraph(IEnumerable<IEdge<TVerticle>> edges )
		{
			Contract.Requires(edges!=null);
			var g = new Graph<TVerticle,TData>(IsDirected);
			foreach(var e in edges)
			{
				TryAddVerticle(e.From,GetData(e.From));
				TryAddVerticle(e.To,GetData(e.To));
				g.AddEdge(e);
			}
			return g;
		}

		public IGraph<TVerticle,TData> GetSubGraph(IEnumerable<TVerticle> verts)
        {
			Contract.Requires(verts!=null);
			var g = new Graph<TVerticle,TData>(IsDirected);
			foreach(var v in verts)
				g.AddVerticle(v,GetData(v));
			foreach(var v in verts)
				foreach(var e in GetEdgesOut(v))
					if(g.ContainsVerticle(e.To))
					    g.AddEdge(e);
			return g;
        }

        private class VerticleHolder
		{
			public TData Data {get;set;}
			public Dictionary<TVerticle,IEdge<TVerticle>> EdgesIn {get;}
			public Dictionary<TVerticle,IEdge<TVerticle>> EdgesOut {get;}
			public VerticleHolder(TData data)
			{
				Data=data;
				EdgesIn = new Dictionary<TVerticle, IEdge<TVerticle>>();
				EdgesOut = new Dictionary<TVerticle, IEdge<TVerticle>>();
			}

		}
	}
}


