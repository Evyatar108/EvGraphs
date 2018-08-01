using System.Collections.Generic;
using System.Collections;
using System.Linq;
using SortedStructures;
using System;
using System.Diagnostics.Contracts;

namespace EvImps.Graphs
{
	public class Graph<TId,TData> : IGraph<TId,TData>
	{
		public bool IsDirected {get;}
		private Dictionary<TId, IVerticle<TId,TData>> verticles;
		public IEnumerable<IVerticle<TId,TData>> Verticles { get{ return verticles.Values;}}
		public IEnumerable<IEdge<IVerticle<TId,TData>>> Edges 
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
			ContainsVerticle(v?.Id);

		public bool ContainsVerticle(TId vId){
		    return verticles.ContainsKey(vId);
		}
        
		public bool AddVerticle(IVerticle<TId,TData> v)=>
			AddVerticle(v?.Id);
        
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
			List<IEdge> edgesToRemove = new List<IEdge>();
			foreach(IEdge e in v.Edges)
				edgesToRemove.Add(e);
			foreach(IEdge e in edgesToRemove)
				RemoveEdge(e);
			verticles.Remove(vId);
			return true;
		}
        
		public bool ContainsEdge(IEdge e)=> ContainsEdge(e.From.Id,e.To.Id);
        
		public bool ContainsEdge(TId vId,TId uId){
			return ContainsVerticle(vId) && ContainsVerticle(uId) 
				&& GetVerticle(vId).IsNeighborOut(uId);
		}

		public bool ContainsRevEdge(IEdge e)=>
		    ContainsEdge(e?.To?.Id,e?.From?.Id);

		public bool ContainsRevEdge(TId vId,TId uId)=>
        ContainsEdge(uId,vId);
        
		public bool AddEdge(IEdge e)=>
			AddEdge(e?.From?.Id,e?.To?.Id, e?.Weight ?? 0);

		public bool AddEdge(TId vId, TId uId, int w=1)
        {
			Contract.Ensures(ContainsEdge(vId,uId));
			if(ContainsEdge(vId,uId))
				return false;
			AddVerticle(vId);
            AddVerticle(uId);
			IVerticle<TId,TData> v = GetVerticle(vId);
			IVerticle<TId,TData> u = GetVerticle(uId);
            IEdge e = new Edge(v,u, w);
            v.AddOutNeighbor(e);
            u.AddInNeighbor(e);
			if(!IsDirected)
			{
				e = new Edge(u,v, w);
				v.AddInNeighbor(e);
                u.AddOutNeighbor(e);
			}
			NumOfEdges++;
			return true;
        }

		public IEdge GetEdge(TId vId,TId uId){
			Contract.Requires(ContainsEdge(vId,uId));
			return GetVerticle(vId).GetEdgeOut(uId);
		}

		public bool RemoveEdge(IEdge e)=>
		    RemoveEdge(e?.From?.Id,e?.To?.Id);

		public bool RemoveEdge(TId vId, TId uId)
		{
			Contract.Ensures(!ContainsEdge(vId,uId));
			if(!ContainsEdge(vId,uId))
				return false;
			IVerticle v = GetVerticle(vId);
			IVerticle u = GetVerticle(uId);
			v.RemoveEdgeOut(uId);
			u.RemoveEdgeIn(vId);
			if(!IsDirected)
			{
				v.RemoveEdgeIn(uId);
                u.RemoveEdgeOut(vId);
			}
			return true;
		}

		public IVerticle GetVerticle(TId vId)
		{
			Contract.Requires(vId!=null);
			Contract.Requires(ContainsVerticle(vId));
			return verticles[vId];
		}

		public void ResetStats()
		{
			foreach (Verticle v in verticles.Values)
				v.ResetStats();
		}
           
		IEnumerator IEnumerable.GetEnumerator() => 
		                       verticles.Values.GetEnumerator();
        
        
		public void Bfs(TId sId)
		{
			Contract.Requires(sId!=null);
			ResetStats();
			var queue = new Queue<IVerticle>();
			IVerticle<TId,TData> v = GetVerticle(sId);
			v.Dist = 0;
			v.IsVisited = true;
			queue.Enqueue(v);
			IVerticle<TId,TData> u;
			while (queue.Count > 0)
			{
				v = queue.Dequeue();
				foreach (Edge e in v.EdgesOut)
				{
					u = e.To;
					if (!u.IsVisited)
					{
						u.IsVisited = true;
						u.Dist = v.Dist + 1;
						u.ParentEdge = e;
						queue.Enqueue(u);
					}
				}
			}
		}

		public void FullDfs()
		{
			ResetStats();
			foreach (IVerticle<TId,TData> v in Verticles)
				if (!v.IsVisited)
					Dfs(v);
		}

		public void Dfs(TId sId)
		{
			Contract.Requires(sId!=null);
			Contract.Requires(ContainsVerticle(sId));
			ResetStats();
			IVerticle s = GetVerticle(sId);
			s.Dist=0;
			Dfs(s);
		}

		private void Dfs(IVerticle<TId,TData> v)
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
					Dfs(u);
				}
			}
		}


		public IGraph<TId,TData> GetMST() //Using Prim
		{
			Contract.Ensures(Contract.Result<Graph>().IsTree());
			if(IsDirected)
				throw new NotSupportedException("No support for directed graphs");
			return Prim();
		}

		private IGraph<TId,TData> Prim()
		{
			if(!IsConnected())
                throw new InvalidOperationException("Graph is not connected");
			ResetStats();
            IVerticle v = verticles.First().Value;
            v.IsVisited = true;
            IEdge[] treeEdges = new IEdge[NumOfVerticles-1];
            int teIndex= 0;
            var edges = new Heap<IEdge>(v.EdgesOut);
            IEdge e;
            while (teIndex<numOfVerticles-1)
            {
                e = GetMinUnvisitedEdge(edges);
                treeEdges[teIndex++] = e;
                v = e.To;
                v.IsVisited=true;
                edges.AddRange(v.EdgesOut);
            }
            return GetSubGraph(treeEdges);
		}

		public bool IsConnected()
		{
			if(NumOfVerticles==0)
				return true;
			Dfs(verticles.First().Value.Id);
			foreach(Verticle v in Verticles)
				if(!v.IsVisited)
					return false;
			return true;
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

		public void Dijkstra(string sId) // Complexity: O(V^2)
		{
			Contract.Requires(sId!=null);
			Contract.Requires(ContainsVerticle(sId));
			ResetStats();
			IVerticle v = verticles[sId];
            v.IsVisited = true;
			v.Dist=0;
			RelaxNeighbores(v);
			for(int i=1;i<numOfVerticles;i++)
            {
				v = FindMinUnvisitedVerticle();
                v.IsVisited=true;
				RelaxNeighbores(v); 
            }
		}

		private static void RelaxNeighbores(IVerticle v)
		{
			foreach(Edge e in v.EdgesOut)
				Relax(e);
		}

		private static void Relax(IEdge e)
		{
			IVerticle v = e.From;
			IVerticle u = e.To;
			if(u.Dist > v.Dist + e.Weight){
				u.Dist = v.Dist + e.Weight;
				u.ParentEdge = e;
			}
		}

		private IVerticle FindMinUnvisitedVerticle(){
			Verticle res = null;
			foreach(Verticle v in verticles.Values)
				if(!v.IsVisited && (res==null || res.Dist > v.Dist) )
					res = v;
            return res;
        }


		public void BellmanFord(string sId)
		{
			Contract.Requires(sId!=null);
			ResetStats();
			GetVerticle(sId).Dist = 0;
			for(int i=0;i<numOfVerticles-1;i++)
				foreach(Edge e in Edges)
					Relax(e);
		}
        
		public bool HasNegativeCycle()
		{
			BellmanFord(Verticles.First().Id);
			foreach(Edge e in Edges)
				if(e.To.Dist > e.From.Dist+e.Weight)
					return true;
			return false;
		}

		public IList<IEdge> GetShortestPath(string sId,string tId,bool statsReady=false)
		{
			Contract.Requires(sId!=null);
			Contract.Requires(ContainsVerticle(sId));
			Contract.Requires(tId!=null);
			Contract.Requires(ContainsVerticle(tId));
			if(!statsReady)
				Bfs(sId);
			IVerticle s = GetVerticle(sId);
			IVerticle v = GetVerticle(tId);
			List<IEdge> path = new List<IEdge>();
			while(v!=s)
			{
				path.Add(v.ParentEdge);
				v= v.ParentEdge.From;
			}
			path.Reverse();
			return path;
		}
        

		public bool IsTree()
		{
			if(IsDirected)
                throw new NotSupportedException("No support for directed graphs");
			if(NumOfEdges != 2*( NumOfVerticles-1))
				return false;
			return IsConnected();
		}

		public IGraph GetSubGraph(IEnumerable<IEdge> edges )
		{
			Contract.Requires(edges!=null);
			Graph g = new Graph(IsDirected);
			foreach(Edge e in edges)
				g.AddEdge(e);
			return g;
		}

		public IGraph GetSubGraph(IEnumerable<IVerticle> verts)
        {
			Contract.Requires(verts!=null);
			Graph g = new Graph(IsDirected);
			foreach(Verticle v in verts)
				g.AddVerticle(v);
			foreach(Verticle v in verts)
				foreach(Edge e in v.EdgesOut)
					if(g.ContainsVerticle(e.To.Id))
					    g.AddEdge(e);
			return g;
        }
	}
}


