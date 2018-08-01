using System.Diagnostics.Contracts;

namespace EvImps.Graphs
{
		public class Edge<TId, TData> : IEdge<TId, TData>
		{
			public IVerticle<TId, TData> From { get; }
			public IVerticle<TId, TData> To { get; }
			public int Weight { get; }
			public Edge(IVerticle<TId,TData> v, IVerticle<TId,TData> u, int w = 1)
			{
				Contract.Requires(v != null);
				Contract.Requires(u != null);
				From = v;
				To = u;
				Weight = w;
			}

			public int CompareTo(IEdge<TId, TData> v) =>
			Weight.CompareTo(v.Weight);

			public override string ToString()
			{
				return $"({From.Id},{To.Id})";
			}

			public EdgeKey GetKey() =>
			new EdgeKey(From.Id.GetHashCode(), To.Id.GetHashCode());
		}






}






