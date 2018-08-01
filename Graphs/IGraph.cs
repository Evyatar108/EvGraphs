using System.Collections.Generic;

namespace EvImps.Graphs
{
	public interface IGraph<TVerticle> : IEnumerable<TVerticle>
		where TVerticle:IVerticle
    {
        bool IsDirected{get;}
		IEnumerable<TVerticle> Verticles{get;}
		IEnumerable<IEdge<TVerticle>> Edges{get;}
        int NumOfEdges{get;}
        int NumOfVerticles { get;}
		bool ContainsVerticle(TVerticle v);
        bool ContainsVerticle(string vId);
		bool AddVerticle(TVerticle v);
        bool AddVerticle(string vId);
        bool RemoveVerticle(string vId);
		bool ContainsEdge(IEdge<TVerticle> e);
        bool ContainsEdge(string vId,string uId);
		bool ContainsRevEdge(IEdge<TVerticle> e);
        bool ContainsRevEdge(string vId,string uId);
		bool AddEdge(IEdge<TVerticle> e);
        bool AddEdge(string vId, string uId, int w);
		IEdge<TVerticle> GetEdge(string vId,string uId);
		bool RemoveEdge(IEdge<TVerticle> e);
        bool RemoveEdge(string vId, string uId);
		TVerticle GetVerticle(string vId);
        void ResetStats();
        void Bfs(string sId);
        void FullDfs();
        void Dfs(string sId);
		IGraph<TVerticle> GetMST();
        bool IsConnected();
        void Dijkstra(string sId);
        void BellmanFord(string sId);
        bool HasNegativeCycle();
		IList<IEdge<TVerticle>> GetShortestPath(string sId,string tId,bool statsReady);
        bool IsTree();
		IGraph<TVerticle> GetSubGraph(IEnumerable<IEdge<TVerticle>> edges );
		IGraph<TVerticle> GetSubGraph(IEnumerable<TVerticle> verts);
    }
}
