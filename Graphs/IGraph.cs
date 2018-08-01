using System.Collections.Generic;

namespace EvImps.Graphs
{
	public interface IGraph<TId,TData>
    {
        bool IsDirected{get;}
		IEnumerable<IVerticle<TId,TData>> Verticles{get;}
		IEnumerable<IEdge<TId,TData>> Edges{get;}
        int NumOfEdges{get;}
        int NumOfVerticles { get;}
		bool ContainsVerticle(IVerticle<TId,TData> v);
		bool ContainsVerticle(TId vId);
		bool AddVerticle(IVerticle<TId,TData> v);
		bool AddVerticle(TId vId);
		bool RemoveVerticle(TId vId);
		bool ContainsEdge(IEdge<TId,TData> e);
		bool ContainsEdge(TId vId,TId uId);
		bool ContainsRevEdge(IEdge<TId,TData> e);
		bool ContainsRevEdge(TId vId,TId uId);
		bool AddEdge(IEdge<TId,TData> e);
		bool AddEdge(TId vId, TId uId, int w);
		IEdge<TId,TData> GetEdge(TId vId,TId uId);
		bool RemoveEdge(IEdge<TId,TData> e);
		bool RemoveEdge(TId vId, TId uId);
		IVerticle<TId,TData> GetVerticle(TId vId);
        void ResetStats();
		IGraph<TId,TData> GetSubGraph(IEnumerable<IEdge<TId,TData>> edges );
		IGraph<TId,TData> GetSubGraph(IEnumerable<IVerticle<TId,TData>> verts);
    }
}
