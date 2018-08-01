using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace EvImps.Graphs
{
    public class FlowNetwork
    {
		public Graph G {get;private set;}
		public Func<Edge,int> c;
		public string sId;
		public string tId;

		public FlowNetwork(Graph g, Func<Edge,int> c,string s,string t)
        {
			this.G = g;
			this.c = c;
			this.sId = s;
			this.tId = t;
        }
        
		public Func<Edge,int> EdmondKarp()//Edmon-karp
        {
            Func<Edge,int> f = (e=>0);
			return EdmondKarp(f);
        }

		public Func<Edge,int> EdmondKarp(Func<Edge,int> f)
		{
			Contract.Requires(f!=null);
			FlowNetwork Nf = BuildResidualNetwork(f);
            Nf.G.Bfs(sId);
            List<Edge> path;
			Func<Edge,int> g;
            while(Nf.G.GetVerticle(tId).Dist<int.MaxValue)
            {
				path = Nf.G.GetShortestPath(sId,tId,statsReady:true);
				g = FindPathFlow(path,Nf.c);
				f = AddFlow(f,g);
				Nf = BuildResidualNetwork(f);
				Nf.G.Bfs(sId);
            }
			return f;
		}

		private Func<Edge,int> AddFlow(Func<Edge,int> f,Func<Edge,int> g)
		{
			var dictGF = new Dictionary<EdgeKey,int>();  
			foreach(Edge e in G.Edges)
			{
				dictGF[e.GetKey()] = f(e)+g(e);
				dictGF[e.GetRevKey()] = -f(e)-g(e);
			}
			return DictToFlow(dictGF);
		}

		private Func<Edge,int> FindPathFlow(List<Edge> path, Func<Edge,int> cf)
		{
			var dictF = new Dictionary<EdgeKey,int>();
			int cfp = FindBottleneck(path,cf);
			foreach(Edge e in path)
			{
				dictF[e.GetKey()]= cfp;
				dictF[e.GetRevKey()] = -cfp;
			}
			return DictToFlow(dictF);
		}

		public static Func<Edge,int> DictToFlow(Dictionary<EdgeKey,int> dict)=>
		    e=> dict.ContainsKey(e.GetKey()) ? dict[e.GetKey()] : 0;

		private int FindBottleneck(List<Edge> path,Func<Edge,int> cap)=>
			path.Aggregate(cap(path.First()),(min,e)=> Math.Min(cap(e),min));

		private FlowNetwork BuildResidualNetwork(Func<Edge,int> f)
		{
			var dictCf = new Dictionary<EdgeKey, int>();
			EdgeKey ek;
			foreach(Edge e in G.Edges)
			{
				ek = e.GetKey();
				dictCf[ek] = c(e)-f(e);
				if(!G.ContainsRevEdge(e))
					dictCf[e.GetRevKey()] = f(e);
			}
			Func<Edge,int> cf = DictToFlow(dictCf);
			var Ef = dictCf.Where(arg => arg.Value>0)
			               .Select(arg=>arg.Key); 
			Graph Gf = new Graph();
			foreach(Verticle v in G.Verticles)
				Gf.AddVerticle(v);
			foreach(EdgeKey edgeKey in Ef)
				Gf.AddEdge(edgeKey.from,edgeKey.to);
			return new FlowNetwork(Gf,cf,sId,tId);
		}

		public Func<Edge,int> Dinitz()
		{
			Func<Edge,int> f = (e=>0);
			return Dinitz(f);
		}

		public Func<Edge,int> Dinitz(Func<Edge,int> f)
		{
			Contract.Requires(f!=null);
			FlowNetwork Nf = BuildResidualNetwork(f);
            Nf.G.Bfs(sId);
			FlowNetwork Lf = BuildLayeredNetwork(Nf);
			Func<Edge,int> b; 
            while(Nf.G.GetVerticle(tId).Dist<int.MaxValue)
			{
				b = FindBlockingFlow(Lf);
				f = AddFlow(f,b);
				Nf = BuildResidualNetwork(f);
				Nf.G.Bfs(sId);
				Lf = BuildLayeredNetwork(Nf);
			}
			return f;
		}

		private Func<Edge,int> FindBlockingFlow(FlowNetwork Lf)
		{
			Func<Edge,int> b = (e=>0);
			Verticle t = Lf.G.GetVerticle(tId);
			List<Edge> path;
			Func<Edge,int> g;
			while(t.NumOfEdgesIn>0)
			{
				path = FindBackwardPath(Lf.G,sId,tId);
				g = FindPathFlow(path,Lf.c);
				b = AddFlow(b,g);
				CleanSaturatedEdges(Lf,b,path);
				Lf.G.Bfs(sId);
			}
			return b;
		}

		private void CleanSaturatedEdges(FlowNetwork Lf,Func<Edge,int> f,List<Edge> path)
		{
			foreach(Edge e in path)
				if(f(e) == Lf.c(e)){
					Lf.G.RemoveEdge(e);
					CleanForward(Lf,e.To);
				}
		}

		private void CleanForward(FlowNetwork Lf,Verticle v)
		{
			if(v.NumOfEdgesIn==0)
				foreach(Edge e in v.EdgesOut.ToList()){
					Lf.G.RemoveEdge(e);
					CleanForward(Lf,e.To);
				}
		}

		private static List<Edge> FindBackwardPath(Graph G,string uId,string vId)
		{
			Verticle r = G.GetVerticle(vId);
			Verticle u = G.GetVerticle(uId);
			var path = new List<Edge>();
			Edge e;
			while(r!=u)
			{
				e = r.EdgesIn.First();
				path.Add(e);
				r = e.From;
			}
			path.Reverse();
			return path;
		}


        //Assuming there is a path between s and t in Nf, and BFS was already done
		private FlowNetwork BuildLayeredNetwork(FlowNetwork Nf)
		{
			Graph Lf = new Graph();
			int numOfLayers = Nf.G.GetVerticle(tId).Dist;
			foreach(Edge e in Nf.G.Edges)
				if(e.To.Dist <= numOfLayers && e.From.Dist+1 == e.To.Dist)
					Lf.AddEdge(e);
			return new FlowNetwork(Lf,Nf.c,sId,tId);
		}

		public bool IsValidFlow(Func<Edge,int> f)
		{
			Contract.Requires(f!=null);
			foreach(Edge e in G.Edges)
				if( f(e) != -f(new Edge(e.To,e.From)))
				   return false;
			return true;

		}

    }
}
