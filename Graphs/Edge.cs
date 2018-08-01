using System.Diagnostics.Contracts;

namespace EvImps.Graphs
{
	public class Edge<TVerticle> : IEdge<TVerticle>
	{
		public TVerticle From { get; }
		public TVerticle To { get; }
		public int Weight { get; }
		public Edge(TVerticle v, TVerticle u, int w = 1)
		{
			From = v;
			To = u;
			Weight = w;
		}

		public int CompareTo(IEdge<TVerticle> e) =>
			Weight.CompareTo(e.Weight);

		public override string ToString()
		{
			return $"({From},{To})";
		}

		public EdgeKey GetKey() =>
		new EdgeKey(From.GetHashCode(), To.GetHashCode());
	}
}






