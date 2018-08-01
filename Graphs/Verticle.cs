using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace EvImps.Graphs
{
	public class Verticle<TId,TData> : IVerticle<TId,TData>
    {
		private Dictionary<TId,IEdge<IVerticle<TId,TData>>> edgesIn;
		public IEnumerable<IEdge<IVerticle<TId,TData>>> EdgesIn{get{return edgesIn.Values;}}

		private Dictionary<TId,IEdge<IVerticle<TId,TData>>> edgesOut;
		public IEnumerable<IEdge<IVerticle<TId,TData>>> EdgesOut{get{return edgesOut.Values;}}

		public IEnumerable<IEdge<IVerticle<TId,TData>>> Edges{get{return EdgesOut.Concat(EdgesIn);}}

        public int NumOfEdgesOut {get{return edgesOut.Count;}}

        public int NumOfEdgesIn {get{return edgesIn.Count;}}


        public int Dist { get; set; } //may be used as a weight of a path
        public IEdge ParentEdge { get; set; }
        public bool IsVisited { get; set; }
		public TId Id { get; }

		public Verticle(TId id)
        {
            Contract.Requires(id!=null);
			edgesIn = new Dictionary<TId,IEdge<IVerticle<TId,TData>>>();
			edgesOut = new Dictionary<TId,IEdge<IVerticle<TId,TData>>>();
            IsVisited = false;
            Id = id;
        }

        public void ResetStats()
        {
            IsVisited = false;
            Dist = int.MaxValue;
            ParentEdge = null;
        }

		public void AddInNeighbor(IEdge<IVerticle<TId,TData>> e){
            Contract.Requires(e!=null);
            Contract.Ensures(Id.Equals(e.To.Id));
            Contract.Ensures(IsNeighborIn(e.From.Id));
            edgesIn.Add(e.From.Id,e);
        }
        
		public void AddOutNeighbor(IEdge<IVerticle<TId,TData>> e){
            Contract.Requires(e!=null);
            Contract.Ensures(Id.Equals(e.From.Id));
            Contract.Ensures(IsNeighborOut(e.To.Id));
            edgesOut.Add(e.To.Id,e);
        }
        
		public bool RemoveEdgeIn(IEdge<IVerticle<TId,TData>> e)=>
        RemoveEdgeIn(e.From.Id);

		public bool RemoveEdgeIn(TId uId){
            Contract.Requires(uId != null);
            return edgesIn.Remove(uId);
        }
        
		public bool RemoveEdgeOut(IEdge<IVerticle<TId,TData>> e)=>
        RemoveEdgeOut(e?.To?.Id);

		public bool RemoveEdgeOut(TId uId){
            Contract.Requires(uId != null);
            return edgesOut.Remove(uId);
        }

		public bool IsNeighborOut(TId uId)=>
        edgesOut.ContainsKey(uId);

		public bool IsNeighborIn(TId uId)=>
        edgesIn.ContainsKey(uId);

		public IEdge<IVerticle<TId,TData>> GetEdgeIn(TId vId){
            Contract.Requires(vId!=null);
            Contract.Requires(IsNeighborIn(vId));
            return edgesIn[vId];
        }

		public IEdge<IVerticle<TId,TData>> GetEdgeOut(TId vId){
            Contract.Requires(vId!=null);
            Contract.Requires(IsNeighborOut(vId));
            return edgesOut[vId];
        }
    }
}
