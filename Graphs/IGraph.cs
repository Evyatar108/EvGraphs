using System.Collections.Generic;

namespace EvImps.Graphs
{
	public interface IGraph<TVerticle,TData>
    {
        bool IsDirected{get;}
		IEnumerable<TVerticle> Verticles{get;}
		IEnumerable<IEdge<TVerticle>> Edges{get;}
		IEnumerable<IEdge<TVerticle>> GetEdgesOut(TVerticle v);
		IEnumerable<IEdge<TVerticle>> GetEdgesIn(TVerticle v);
		TData GetData(TVerticle v);
		void SetData(TVerticle v, TData data);
        int NumOfEdges{get;}
        int NumOfVerticles { get;}
		bool ContainsVerticle(TVerticle v);
		bool TryAddVerticle(TVerticle v, TData data);
		void AddVerticle(TVerticle v,TData data);
		bool TryRemoveVerticle(TVerticle v);
		void RemoveVerticle(TVerticle v);
		bool ContainsEdge(IEdge<TVerticle> e);
		bool ContainsEdge(TVerticle v,TVerticle u);
		bool ContainsReverseEdge(IEdge<TVerticle> e);
		bool ContainsReverseEdge(TVerticle v,TVerticle u);
		bool TryAddEdge(IEdge<TVerticle> e);
		void AddEdge(IEdge<TVerticle> e);
		void AddEdge(TVerticle v, TVerticle u, int w);
		IEdge<TVerticle> GetEdge(TVerticle v,TVerticle u);
		void RemoveEdge(IEdge<TVerticle> e);
		void RemoveEdge(TVerticle v, TVerticle u);
		IGraph<TVerticle,TData> GetSubGraph(IEnumerable<IEdge<TVerticle>> edges );
		IGraph<TVerticle,TData> GetSubGraph(IEnumerable<TVerticle> verts);
    }
}
