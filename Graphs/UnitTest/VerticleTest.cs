using NUnit.Framework;
using System;
using System.Collections.Generic;
namespace EvImps.Graphs.UnitTest
{
	[TestFixture()]
	public class VerticleTest
	{
		[Test()]
		public void TestAddNeighbor()
		{
			Verticle v = new Verticle("v");
			Verticle u = new Verticle("u");
			Assert.Throws<KeyNotFoundException>(()=> v.GetEdgeIn("u"));
			Assert.AreEqual(v.NumOfEdgesIn, 0);
			Assert.AreEqual(v.NumOfEdgesOut, 0);
			Edge e = new Edge(v, u);
			v.AddOutNeighbor(e);
			u.AddInNeighbor(e);
			Assert.IsTrue(v.IsNeighborOut("u"));
			Assert.IsTrue(u.IsNeighborIn("v"));
			Assert.AreEqual(v.NumOfEdgesIn, 0);
			Assert.AreEqual(v.NumOfEdgesOut, 1);
			Assert.AreEqual(u.NumOfEdgesIn, 1);
			Assert.AreEqual(u.NumOfEdgesOut, 0);
			Assert.AreSame(v.GetEdgeOut("u"),e);
			Assert.AreSame(u.GetEdgeIn("v"),e);
			Assert.Throws<ArgumentException>(()=>v.AddOutNeighbor(e));
			Assert.Throws<ArgumentException>(()=>u.AddInNeighbor(e));
		}

		[Test()]
		public void TestRemoveNeighbor()
		{
			Verticle v = new Verticle("v");
			Verticle u = new Verticle("u");
			Edge e = new Edge(v, u);
			v.AddOutNeighbor(e);
			u.AddInNeighbor(e);
			Assert.IsFalse(v.RemoveEdgeIn("lala"));
			Assert.IsFalse(u.RemoveEdgeIn("lala"));
			Assert.IsFalse(v.RemoveEdgeOut("lala"));
			Assert.IsFalse(u.RemoveEdgeOut("lala"));
			Assert.IsTrue(v.RemoveEdgeOut("u"));
			Assert.IsTrue(u.RemoveEdgeIn("v"));
			Assert.AreEqual(v.NumOfEdgesIn, 0);
			Assert.AreEqual(v.NumOfEdgesOut, 0);
			Assert.IsFalse(v.IsNeighborOut("u"));
			Assert.IsFalse(u.IsNeighborIn("v"));
			Assert.Throws<KeyNotFoundException>(()=> v.GetEdgeIn("u"));
			Assert.Throws<KeyNotFoundException>(()=> v.GetEdgeOut("u"));
            Assert.Throws<KeyNotFoundException>(()=> u.GetEdgeIn("v"));
            Assert.Throws<KeyNotFoundException>(()=> u.GetEdgeOut("v"));
		}

		public void TestEdges()
		{
			Verticle v = new Verticle("v");
            Verticle u = new Verticle("u");
            Edge e = new Edge(v, u);
            v.AddOutNeighbor(e);
            u.AddInNeighbor(e);
			foreach(Edge edge in v.EdgesOut)
				Assert.AreSame(e,edge);
			foreach(var x in v.EdgesIn)
				Assert.Fail();
			foreach(Edge edge in u.EdgesIn)
                Assert.AreSame(e,edge);
            foreach(var x in u.EdgesOut)
                Assert.Fail();
		}
	}
}
