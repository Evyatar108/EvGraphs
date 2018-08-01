using System;

namespace EvImps.Graphs
{
   
	public interface IEdge<TVerticle> : IComparable<IEdge<TVerticle>>

	{
		TVerticle From { get; }
		TVerticle To { get; }
		int Weight { get; }
        EdgeKey GetKey();
    }
}
