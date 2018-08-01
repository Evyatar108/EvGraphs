using System.Diagnostics.Contracts;

namespace EvImps.Graphs
{
	public class Edge<TVerticle> : IEdge<TVerticle> where TVerticle:IVerticle
	{
		public IVerticle From { get; }
		public IVerticle To{ get; }
        public int Weight { get; }
		public Edge(IVerticle v, IVerticle u, int w=1)
        {
			Contract.Requires(v!=null);
			Contract.Requires(u!=null);
            From = v;
            To = u;
            Weight = w;
        }

		public int CompareTo(IEdge v)=>
        Weight.CompareTo(v.Weight);

		public override string ToString()
        {
			return $"({From.Id},{To.Id})";
        }

		public EdgeKey GetKey()=>
        new EdgeKey(From.Id,To.Id);
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

		public EdgeKey Rev()=>
		new EdgeKey(to,from);
        

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






