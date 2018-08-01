using System;

namespace EvImps.Graphs
{
   
	public interface IEdge<TId,TData> : IComparable<IEdge<TId,TData>>

	{
		IVerticle<TId,TData> From { get; }
		IVerticle<TId,TData> To { get; }
		int Weight { get; }
        EdgeKey GetKey();
    }
}
