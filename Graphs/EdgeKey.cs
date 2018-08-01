using System;
namespace EvImps.Graphs
{
	public struct EdgeKey
    {
        public readonly object fromId;
        public readonly object toId;

        public EdgeKey(object fromId, object toId)
        {
            this.fromId = fromId;
            this.toId = toId;
        }

        public EdgeKey Rev() =>
        new EdgeKey(toId, fromId);


        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + fromId.GetHashCode();
                hash = hash * 23 + toId.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj) =>
        obj is EdgeKey
        && ((EdgeKey)obj).fromId == fromId
                             && ((EdgeKey)obj).toId == toId;
    }
}
