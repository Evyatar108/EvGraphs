using System;

namespace EvImps.Graphs
{
	public class Edge : IComparable<Edge> //Undirected
    {
        public Verticle From { get; }
        public Verticle To{ get; }
        public int Weight { get; }
        public Edge(Verticle v, Verticle u, int w=1)
        {
            From = v;
            To = u;
            Weight = w;
        }

		public override string ToString()
        {
			return $"({From.Id},{To.Id})";
        }

        public int CompareTo(Edge e)=>
        Weight.CompareTo(e.Weight);

		public EdgeKey GetKey()=>
        new EdgeKey(From.Id,To.Id);

		public EdgeKey GetRevKey()=>
        new EdgeKey(To.Id,From.Id);
	}



	public struct EdgeKey
    {
        public readonly string from;
        public readonly string to;
        public EdgeKey(string from,string to)
        {
            this.from = from;
            this.to = to;
        }



		public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + from.GetHashCode();
                hash = hash * 23 + to.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)=>
        obj is EdgeKey 
        && ((EdgeKey)obj).from == from
                     && ((EdgeKey)obj).to == to;
    }
}






