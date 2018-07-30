using System.Collections.Generic;
using System.Collections;
using System.Linq;
using SortedStructures;
using System;
using System.Diagnostics.Contracts;

namespace EvImps.Graphs
{
	public class Graph : IEnumerable
	{
		public bool IsDirected {get;set;}
		internal Dictionary<string, Verticle> verticles;
		public IEnumerable<Verticle> Verticles { get{ return verticles.Values;}}
		public IEnumerable<Edge> Edges 
		{
			get 
			{ 
				foreach(Verticle v in verticles.Values)
					foreach(Edge e in v.EdgesOut)
						yield return e;
			}
		}

		public int NumOfEdges
		{
			get
			{
				return Verticles.Aggregate(0,(sum,v)=> sum+ v.EdgesOut.Count);
			}
		}

		int numOfVerticles;
		public int NumOfVerticles { get { return numOfVerticles; } }
        
		public Graph(bool isDirected=true)
		{
			verticles = new Dictionary<string, Verticle>();
			numOfVerticles = 0;
			IsDirected = isDirected;
		}

		public bool ContainsVerticle(Verticle v)=>
			ContainsVerticle(v?.Id);

		public bool ContainsVerticle(string vId){
			Contract.Requires(vId!=null);
		    return verticles.ContainsKey(vId);
		}

		public bool AddVerticle(Verticle v)=>
			AddVerticle(v?.Id);
        
		public bool AddVerticle(string vId)
		{
			Contract.Requires(vId!=null);
			Contract.Ensures(ContainsVerticle(vId));
			if (ContainsVerticle(vId))
				return false;
			numOfVerticles++;
			verticles[vId] = new Verticle(vId);
			return true;
		}

		public bool RemoveVerticle(string vId)
		{
			Contract.Requires(vId!=null);
			Contract.Ensures(!ContainsVerticle(vId));
			if(!verticles.TryGetValue(vId,out Verticle v))
				return false;
			List<Edge> edgesToRemove = new List<Edge>();
			foreach(Edge e in v.Edges)
				edgesToRemove.Add(e);
			foreach(Edge e in edgesToRemove)
				RemoveEdge(e);
			verticles.Remove(vId);
			return true;
		}
        
		public bool ContainsEdge(Edge e)=> ContainsEdge(e?.From?.Id,e?.To?.Id);
        
		public bool ContainsEdge(string vId,string uId){
			Contract.Requires(vId!=null);
			Contract.Requires(uId!=null);
			return ContainsVerticle(vId) && ContainsVerticle(uId) 
				&& GetVerticle(vId).IsNeighborOut(uId);
		}

		public bool ContainsRevEdge(Edge e)=>
		    ContainsEdge(e?.To?.Id,e?.From?.Id);

		public bool ContainsRevEdge(string vId,string uId)=>
        ContainsEdge(uId,vId);
        
		public bool AddEdge(Edge e)=>
			AddEdge(e?.From?.Id,e?.To?.Id, e?.Weight ?? 0);

		public bool AddEdge(string vId, string uId, int w=1)
        {
			Contract.Requires(vId!=null);
			Contract.Requires(uId!=null);
			Contract.Ensures(ContainsEdge(vId,uId));
			if(ContainsEdge(vId,uId))
				return false;
			AddVerticle(vId);
            AddVerticle(uId);
			Verticle v = GetVerticle(vId);
            Verticle u = GetVerticle(uId);
            Edge e = new Edge(v,u, w);
            v.AddOutNeighbor(e);
            u.AddInNeighbor(e);
			if(!IsDirected)
			{
				e = new Edge(u,v, w);
				v.AddInNeighbor(e);
                u.AddOutNeighbor(e);
			}
			return true;
        }

		public Edge GetEdge(string vId,string uId){
			Contract.Requires(vId!=null);
			Contract.Requires(uId!=null);
			Contract.Requires(ContainsEdge(vId,uId));
			return GetVerticle(vId).GetEdgeOut(uId);
		}

		public bool RemoveEdge(Edge e)=>
		    RemoveEdge(e?.From?.Id,e?.To?.Id);

		public bool RemoveEdge(string vId, string uId)
		{
			Contract.Requires(vId!=null);
			Contract.Requires(uId!=null);
			Contract.Ensures(!ContainsEdge(vId,uId));
			if(!ContainsEdge(vId,uId))
				return false;
			Verticle v = GetVerticle(vId);
			Verticle u = GetVerticle(uId);
			v.RemoveEdgeOut(uId);
			u.RemoveEdgeIn(vId);
			if(!IsDirected)
			{
				v.RemoveEdgeIn(uId);
                u.RemoveEdgeOut(vId);
			}
			return true;
		}

		public Verticle GetVerticle(string vId)
		{
			Contract.Requires(vId!=null);
			Contract.Requires(ContainsVerticle(vId));
			return verticles[vId];
		}

		private void ResetStats()
		{
			foreach (Verticle v in verticles.Values)
				v.ResetStats();
		}
           
		IEnumerator IEnumerable.GetEnumerator() => 
		    verticles.Values.GetEnumerator();

        
		public void Bfs(string sId)
		{
			Contract.Requires(sId!=null);
			ResetStats();
			var queue = new Queue<Verticle>();
			Verticle v = GetVerticle(sId);
			v.Dist = 0;
			v.Visited = true;
			queue.Enqueue(v);
			Verticle u;
			while (queue.Count > 0)
			{
				v = queue.Dequeue();
				foreach (Edge e in v.EdgesOut)
				{
					u = e.To;
					if (!u.Visited)
					{
						u.Visited = true;
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
			foreach (Verticle v in Verticles)
				if (!v.Visited)
					Dfs(v);
		}

		public void Dfs(string sId)
		{
			Contract.Requires(sId!=null);
			Contract.Requires(ContainsVerticle(sId));
			ResetStats();
			Verticle s = GetVerticle(sId);
			s.Dist=0;
			Dfs(s);
		}

		private void Dfs(Verticle v)
		{
			v.Visited = true;
			Verticle u;
			foreach (Edge e in v.EdgesOut)
			{
				u = e.To;
				if (!u.Visited)
				{
					u.ParentEdge = e;
					u.Dist = v.Dist+1;
					Dfs(u);
				}
			}
		}


		public Graph GetMST() //Using Prim
		{
			Contract.Ensures(Contract.Result<Graph>().IsTree());
			if(IsDirected)
				throw new NotSupportedException("No support for directed graphs");
			return Prim();
		}

		private Graph Prim()
		{
			if(!IsConnected())
                throw new InvalidOperationException("Graph is not connected");
			ResetStats();
            Verticle v = verticles.First().Value;
            v.Visited = true;
            Edge[] treeEdges = new Edge[NumOfVerticles-1];
            int teIndex= 0;
            var edges = new Heap<Edge>(v.EdgesOut);
            Edge e;
            while (teIndex<numOfVerticles-1)
            {
                e = GetMinUnvisitedEdge(edges);
                treeEdges[teIndex++] = e;
                v = e.To;
                v.Visited=true;
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
				if(!v.Visited)
					return false;
			return true;
		}

		private Edge GetMinUnvisitedEdge(Heap<Edge> edges){
			bool found = false;
			Edge e=null;
			while(!found)
			{
				e = edges.ExtractMin();
				found = !e.To.Visited;
			}
			return e;
		}

		public void Dijkstra(string sId) // Complexity: O(V^2)
		{
			Contract.Requires(sId!=null);
			Contract.Requires(ContainsVerticle(sId));
			ResetStats();
			Verticle v = verticles[sId];
            v.Visited = true;
			v.Dist=0;
			RelaxNeighbores(v);
			for(int i=1;i<numOfVerticles;i++)
            {
				v = FindMinUnvisitedVerticle();
                v.Visited=true;
				RelaxNeighbores(v); 
            }
		}

		private static void RelaxNeighbores(Verticle v)
		{
			foreach(Edge e in v.EdgesOut)
				Relax(e);
		}

		private static void Relax(Edge e)
		{
			Verticle v = e.From;
			Verticle u = e.To;
			if(u.Dist > v.Dist + e.Weight){
				u.Dist = v.Dist + e.Weight;
				u.ParentEdge = e;
			}
		}

		private Verticle FindMinUnvisitedVerticle(){
			Verticle res = null;
			foreach(Verticle v in verticles.Values)
				if(!v.Visited && (res==null || res.Dist > v.Dist) )
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

		public List<Edge> GetShortestPath(string sId,string tId,bool statsReady=false)
		{
			Contract.Requires(sId!=null);
			Contract.Requires(ContainsVerticle(sId));
			Contract.Requires(tId!=null);
			Contract.Requires(ContainsVerticle(tId));
			if(!statsReady)
				Bfs(sId);
			Verticle s = GetVerticle(sId);
			Verticle v = GetVerticle(tId);
			List<Edge> path = new List<Edge>();
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

		public Graph GetSubGraph(IEnumerable<Edge> edges )
		{
			Contract.Requires(edges!=null);
			Graph g = new Graph(IsDirected);
			foreach(Edge e in edges)
				g.AddEdge(e);
			return g;
		}

		public Graph GetSubGraph(IEnumerable<Verticle> verts)
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


