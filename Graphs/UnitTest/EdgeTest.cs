using NUnit.Framework;
using System;
namespace EvImps.Graphs.UnitTest
{
    [TestFixture()]
    public class EdgeTest
    {
        [Test()]
        public void TestCase()
        {
			Verticle v = new Verticle("v");
			Verticle u = new Verticle("u");
			Edge e = new Edge(v,u);
			Assert.AreSame(e.From,v);
			Assert.AreSame(e.To,u);
			EdgeKey edgeKey = e.GetKey();
			Assert.AreEqual(edgeKey.from,v.Id,"v");
			Assert.AreEqual(edgeKey.to,u.Id,"u");
        }

		public void TestEdgeKey()
		{
			EdgeKey first = new EdgeKey("x","y");
			EdgeKey second = new EdgeKey("x","y");
			Assert.AreEqual(first,second);
			Assert.AreEqual(first.GetHashCode(),second.GetHashCode());
		}
    }
}
