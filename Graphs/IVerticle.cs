using System.Collections.Generic;

namespace EvImps.Graphs
{
    
    public interface IVerticle<TId,TData> 
    {
        IEnumerable<IEdge<TId,TData>> EdgesIn{get;}
        IEnumerable<IEdge<TId,TData>> EdgesOut{get;}
        IEnumerable<IEdge<TId,TData>> Edges{get;}
        int NumOfEdgesOut {get;}
        int NumOfEdgesIn {get;}
        int Dist { get;set;}
        IEdge<TId,TData> ParentEdge { get; set; }
        bool IsVisited { get; set; }
        TId Id { get; }
        void ResetStats();
        void AddInNeighbor(IEdge<TId,TData> e);
        void AddOutNeighbor(IEdge<TId,TData> e);
        bool RemoveEdgeOut(IEdge<TId,TData> e);
        bool RemoveEdgeOut(TId uId);
        bool RemoveEdgeIn(IEdge<TId,TData> e);
        bool RemoveEdgeIn(TId uId);
        bool IsNeighborOut(TId uId);
        bool IsNeighborIn(TId uId);
        IEdge<TId,TData> GetEdgeIn(TId vId);
        IEdge<TId,TData> GetEdgeOut(TId vId);
    }
}
