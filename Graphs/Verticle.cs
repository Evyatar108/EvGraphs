using System.Collections.Generic;
using System.Linq;

namespace EvImps.Graphs
{
	public class Verticle  //Undirected
    {
		private Dictionary<string,Edge> edgesIn;
		public ICollection<Edge> EdgesIn{get{return edgesIn.Values;}}

		private Dictionary<string,Edge> edgesOut;
		public ICollection<Edge> EdgesOut{get{return edgesOut.Values;}}

		public IEnumerable<Edge> Edges{get{return EdgesOut.Concat(EdgesIn);}}

		public int NumOfEdgesOut {get{return edgesOut.Count;}}

		public int NumOfEdgesIn {get{return edgesIn.Count;}}


        public int Dist { get; internal set; } //may be used as a weight of a path
        public Edge ParentEdge { get; set; }
        internal bool Visited { get; set; }
        public string Id { get; }

        public Verticle(string id)
        {
            edgesIn = new Dictionary<string,Edge>();
			edgesOut = new Dictionary<string,Edge>();
            Visited = false;
            Id = id;
        }

        public void ResetStats()
        {
            Visited = false;
            Dist = int.MaxValue;
            ParentEdge = null;
        }

        public void AddInNeighbor(Edge e) =>
		edgesIn.Add(e.From.Id,e);
        
        
        public void AddOutNeighbor(Edge e) =>
	    edgesOut.Add(e.To.Id,e);
        
        public bool RemoveEdgeIn(Edge e)=>
		RemoveEdgeIn(e.From.Id);

        public bool RemoveEdgeIn(string uId)=>
        edgesIn.Remove(uId);
        
		public bool RemoveEdgeOut(Edge e)=>
        RemoveEdgeOut(e.To.Id);

        public bool RemoveEdgeOut(string uId)=>
		edgesOut.Remove(uId);

		public bool IsNeighborOut(string uId)=>
		edgesOut.ContainsKey(uId);

		public bool IsNeighborIn(string uId)=>
        edgesIn.ContainsKey(uId);

		public Edge GetEdgeIn(string vId)=>
		edgesIn[vId];

		public Edge GetEdgeOut(string vId)=>
        edgesOut[vId];
    }
}
