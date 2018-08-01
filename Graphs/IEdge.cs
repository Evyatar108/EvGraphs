using System;

namespace EvImps.Graphs
{

	public interface IEdge : IComparable<IEdge>
    {
    }

	public interface IEdge<TVerticle> : IEdge where TVerticle : IVerticle
	{
		TVerticle From { get; }
		TVerticle To { get; }
		int Weight { get; }
        EdgeKey GetKey();
    }
}
