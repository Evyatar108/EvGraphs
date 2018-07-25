using NUnit.Framework;
using System.Collections.Generic;
namespace EvImps.Graphs.UnitTest
{
	[TestFixture()]
	public class FlowNetworkTest
	{

		public static FlowNetwork BuildComplexFlowNetwork()
		{
			Graph g = new Graph();
			g.AddEdge("s", "a");
			g.AddEdge("s", "b");
			g.AddEdge("a", "c");
			g.AddEdge("b", "a");
			g.AddEdge("b", "c");
			g.AddEdge("c", "d");
			g.AddEdge("c", "t");
			g.AddEdge("d", "b");
			g.AddEdge("d", "t");
			var dictC = new Dictionary<EdgeKey, int>();
			dictC[g.GetEdge("s", "a").GetKey()]=4;
			dictC[g.GetEdge("s", "b").GetKey()]=8;
			dictC[g.GetEdge("a", "c").GetKey()]=6;
			dictC[g.GetEdge("b", "a").GetKey()]=3;
			dictC[g.GetEdge("b", "c").GetKey()]=6;
			dictC[g.GetEdge("c", "d").GetKey()]=3;
			dictC[g.GetEdge("c", "t").GetKey()]=11;
			dictC[g.GetEdge("d", "b").GetKey()]=1;
			dictC[g.GetEdge("d", "t").GetKey()]=2;
			var c = FlowNetwork.DictToFlow(dictC);
			return new FlowNetwork(g,c,"s","t");
		}

		public static FlowNetwork BuildSimpleFlowNetwork()
		{
			Graph g = new Graph();
            g.AddEdge("s", "a");
            g.AddEdge("s", "b");
            g.AddEdge("a", "t");
            g.AddEdge("b", "t");
            var dictC = new Dictionary<EdgeKey, int>();
            dictC[g.GetEdge("s", "a").GetKey()]=4;
            dictC[g.GetEdge("s", "b").GetKey()]=8;
            dictC[g.GetEdge("a", "t").GetKey()]=1;
            dictC[g.GetEdge("b", "t").GetKey()]=1;
            var c = FlowNetwork.DictToFlow(dictC);
            return new FlowNetwork(g,c,"s","t");
		}


		[Test()]
        public void TestEdmondKarp()
        {
			FlowNetwork N = BuildSimpleFlowNetwork();
            var f = N.EdmondKarp();
			Assert.AreEqual(1,f(N.G.GetEdge("s","a")));
			Assert.AreEqual(1,f(N.G.GetEdge("s","b")));
			Assert.AreEqual(1,f(N.G.GetEdge("a","t")));
			Assert.AreEqual(1,f(N.G.GetEdge("b","t")));
            Assert.IsTrue(N.IsValidFlow(f));

			N = BuildComplexFlowNetwork();
			f = N.EdmondKarp();
			Assert.AreEqual(12,f(N.G.GetEdge("s","a"))+f(N.G.GetEdge("s","b")));
			Assert.AreEqual(12,f(N.G.GetEdge("c","t"))+f(N.G.GetEdge("d","t")));
            Assert.IsTrue(N.IsValidFlow(f));
        }

		[Test()]
		public void TestDinitz()
		{
			FlowNetwork N = BuildSimpleFlowNetwork();
			var f = N.Dinitz();
			Assert.AreEqual(1,f(N.G.GetEdge("s","a")));
            Assert.AreEqual(1,f(N.G.GetEdge("s","b")));
            Assert.AreEqual(1,f(N.G.GetEdge("a","t")));
            Assert.AreEqual(1,f(N.G.GetEdge("b","t")));
            Assert.IsTrue(N.IsValidFlow(f));

            N = BuildComplexFlowNetwork();
			f = N.Dinitz();
			Assert.AreEqual(12,f(N.G.GetEdge("s","a"))+f(N.G.GetEdge("s","b")));
            Assert.AreEqual(12,f(N.G.GetEdge("c","t"))+f(N.G.GetEdge("d","t")));
            Assert.IsTrue(N.IsValidFlow(f));
		}
	}
}
