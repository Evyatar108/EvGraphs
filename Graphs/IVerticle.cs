using System.Collections.Generic;

namespace EvImps.Graphs
{
    public interface IVerticle
    {
    }
    
    public interface IVerticle<TId,TData> : IVerticle
    {
        IEnumerable<IEdge<IVerticle<TId,TData>>> EdgesIn{get;}
        IEnumerable<IEdge<IVerticle<TId,TData>>> EdgesOut{get;}
        IEnumerable<IEdge<IVerticle<TId,TData>>> Edges{get;}
        int NumOfEdgesOut {get;}
        int NumOfEdgesIn {get;}
        int Dist { get;set;}
        IEdge<IVerticle<TId,TData>> ParentEdge { get; set; }
        bool IsVisited { get; set; }
        TId Id { get; }
        void ResetStats();
        void AddInNeighbor(IEdge<IVerticle<TId,TData>> e);
        void AddOutNeighbor(IEdge<IVerticle<TId,TData>> e);
        bool RemoveEdgeOut(IEdge<IVerticle<TId,TData>> e);
        bool RemoveEdgeOut(TId uId);
        bool RemoveEdgeIn(IEdge<IVerticle<TId,TData>> e);
        bool RemoveEdgeIn(TId uId);
        bool IsNeighborOut(TId uId);
        bool IsNeighborIn(TId uId);
        IEdge<IVerticle<TId,TData>> GetEdgeIn(TId vId);
        IEdge<IVerticle<TId,TData>> GetEdgeOut(TId vId);
    }
}
