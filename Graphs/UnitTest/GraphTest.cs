using NUnit.Framework;
using System.Collections.Generic;
using System;
namespace EvImps.Graphs.UnitTest
{
    [TestFixture()]
    public class GraphTest
    {
		Graph g;

		[SetUp()]
		public void BeforeTests()
		{
			g = new Graph(true);
		}

        [Test()]
        public void TestAddVerticle()
        {
			Assert.Throws<KeyNotFoundException>(()=> g.GetVerticle("v"));
			Assert.IsFalse(g.ContainsVerticle("v"));
			Assert.IsTrue(g.AddVerticle("v"));
			Assert.IsTrue(g.ContainsVerticle("v"));
			Assert.IsFalse(g.AddVerticle("v"));
			Assert.AreEqual("v", g.GetVerticle("v").Id);
        }

		[Test()]
		public void TestRemoveVerticle()
		{
			Assert.IsFalse(g.RemoveVerticle("v"));
			g.AddVerticle("v");
			Assert.IsTrue(g.RemoveVerticle("v"));
            Assert.IsFalse(g.ContainsVerticle("v"));
            Assert.IsFalse(g.RemoveVerticle("v"));
			Assert.Throws<KeyNotFoundException>(()=> g.GetVerticle("v"));
			g.AddVerticle("v");
			g.AddVerticle("u");
			g.AddEdge("v","u");
			g.RemoveVerticle("v");
			Assert.IsFalse(g.ContainsEdge("v","u"));
		}

		[Test()]
		public void TestAddEdge()
		{
			Assert.IsFalse(g.ContainsEdge("v","u"));
			g.AddVerticle("v");
            g.AddVerticle("u");
			Assert.IsFalse(g.ContainsEdge("v","u"));
            Assert.IsTrue(g.AddEdge("v","u"));
			Assert.IsTrue(g.ContainsEdge("v","u"));
			Assert.IsTrue(g.ContainsRevEdge("u","v"));
			Assert.IsFalse(g.AddEdge("v","u"));
		}

		[Test()]
		public void TestAddEdgeStrong()
		{
			Assert.AreEqual(0,g.NumOfEdges);
			Assert.IsTrue(g.AddEdge("v","u"));
			Assert.IsTrue(g.ContainsEdge("v","u"));
			Assert.IsTrue(g.ContainsVerticle("v"));
			Assert.IsTrue(g.ContainsVerticle("u"));
			Assert.AreEqual(2,g.NumOfVerticles);
			Assert.AreEqual(1,g.NumOfEdges);
			Assert.IsFalse(g.AddEdge("v","u"));
		}

		[Test()]
		public void TestRemoveEdge()
		{
			Assert.IsFalse(g.RemoveEdge("u","v"));
			g.AddVerticle("v");
			g.AddVerticle("u");
			Assert.IsFalse(g.RemoveEdge("u","v"));
			g.AddEdge("u","v");
			Assert.IsTrue(g.RemoveEdge("u","v"));
			Assert.IsFalse(g.ContainsEdge("u","v"));
			Assert.IsFalse(g.RemoveEdge("u","v"));
		}

		[Test()]
		public void TestGetVerticle()
		{
			Assert.Throws<KeyNotFoundException>(()=> g.GetVerticle("v"));
			g.AddVerticle("v");
			Verticle v = null;
			try
			{
				v = g.GetVerticle("v");
			}catch(Exception exc)
			{
				Assert.Fail();
			}
		}

		[Test()]
		public void TestGetEdge()
		{
			Assert.Throws<KeyNotFoundException>(()=> g.GetEdge("v","u"));
			g.AddVerticle("v");
			g.AddVerticle("u");
			g.AddEdge("v","u");
			Edge e = null;
			try
			{
				e = g.GetEdge("v","u");
			}catch(Exception exc)
			{
				Assert.Fail();
			}
		}


		[Test()]
		public void TestBfs()
		{
			g.AddVerticle("s");
			g.Bfs("s");
			Assert.AreEqual(0,g.GetVerticle("s").Dist);
			g.AddVerticle("v1");
            g.Bfs("s");
			Assert.AreEqual(0,g.GetVerticle("s").Dist);
			Assert.AreEqual(g.GetVerticle("v1").Dist,int.MaxValue);
			g.AddEdge("s","v1");
			g.Bfs("s");
			Assert.AreEqual(0,g.GetVerticle("s").Dist);
            Assert.AreEqual(1,g.GetVerticle("v1").Dist);
			g.AddVerticle("u1");
			g.AddEdge("s","u1");
            g.Bfs("s");
			Assert.AreEqual(0,g.GetVerticle("s").Dist);
            Assert.AreEqual(1,g.GetVerticle("v1").Dist);
			Assert.AreEqual(1,g.GetVerticle("u1").Dist);
			g.AddEdge("v1","u1");
			g.Bfs("s");
			Assert.AreEqual(0,g.GetVerticle("s").Dist);
			Assert.AreEqual(1,g.GetVerticle("v1").Dist);
            Assert.AreEqual(1,g.GetVerticle("u1").Dist);
			g.AddVerticle("v2");
			g.AddVerticle("v3");
			g.AddEdge("v1","v2");
			g.AddEdge("v2","v3");
			g.Bfs("s");
            Assert.AreEqual(0,g.GetVerticle("s").Dist);
            Assert.AreEqual(1,g.GetVerticle("v1").Dist);
            Assert.AreEqual(1,g.GetVerticle("u1").Dist);
			Assert.AreEqual(2,g.GetVerticle("v2").Dist);
			Assert.AreEqual(3,g.GetVerticle("v3").Dist);
		}
        
		[Test()]
		public void TestDfs()
		{
			g.AddVerticle("s");
			g.Dfs("s");
			Verticle s = g.GetVerticle("s");
			Assert.AreEqual(0,s.Dist);
			Assert.AreEqual(s.ParentEdge,null);
			g.AddVerticle("v1");
			Verticle v1 = g.GetVerticle("v1");
			g.AddEdge("s","v1");
			g.Dfs("s");
			Assert.AreEqual(0,s.Dist);
            Assert.AreEqual(s.ParentEdge,null);
			Assert.AreEqual(1,v1.Dist);
			Assert.AreSame(v1.ParentEdge.From,s);
			Assert.AreSame(v1.ParentEdge.To,v1);
			g.AddEdge("v1","v2");
			Verticle v2 = g.GetVerticle("v2");
            g.Dfs("s");
            Assert.AreEqual(0,s.Dist);
            Assert.AreEqual(s.ParentEdge,null);
            Assert.AreEqual(1,v1.Dist);
            Assert.AreSame(v1.ParentEdge.From,s);
            Assert.AreSame(v1.ParentEdge.To,v1);
			Assert.AreEqual(2,v2.Dist);
            Assert.AreSame(v2.ParentEdge.From,v1);
            Assert.AreSame(v2.ParentEdge.To,v2);
			g.AddEdge("v2","s");
			g.Dfs("s");
            Assert.AreEqual(0,s.Dist);
            Assert.AreEqual(s.ParentEdge,null);
            Assert.AreEqual(1,v1.Dist);
            Assert.AreSame(v1.ParentEdge.From,s);
            Assert.AreSame(v1.ParentEdge.To,v1);
            Assert.AreEqual(2,v2.Dist);
            Assert.AreSame(v2.ParentEdge.From,v1);
            Assert.AreSame(v2.ParentEdge.To,v2);
		}

		[Test()]
		public void TestMst()
		{
			g.IsDirected=false;
			g.AddEdge("s","v1",1);
			g.AddEdge("v1","v2",1);
			g.AddEdge("s","u1",1);
			g.AddEdge("v2","v3",1);
			Graph t = g.GetMST();
			Assert.IsTrue(t.ContainsVerticle("s"));
			Assert.IsTrue(t.ContainsVerticle("v1"));
			Assert.IsTrue(t.ContainsVerticle("v2"));
			Assert.IsTrue(t.ContainsVerticle("v3"));
			Assert.IsTrue(t.ContainsVerticle("u1"));
			Assert.AreEqual(5,t.NumOfVerticles);
			Assert.IsTrue(t.IsTree());
			g.AddEdge("v3","s",2);
			t = g.GetMST();
			Assert.IsTrue(t.ContainsVerticle("s"));
            Assert.IsTrue(t.ContainsVerticle("v1"));
            Assert.IsTrue(t.ContainsVerticle("v2"));
            Assert.IsTrue(t.ContainsVerticle("v3"));
            Assert.IsTrue(t.ContainsVerticle("u1"));
            Assert.AreEqual(5,t.NumOfVerticles);
            Assert.IsTrue(t.IsTree());
			Assert.IsFalse(t.ContainsEdge("v3","s"));
			Assert.IsFalse(t.ContainsEdge("s","v3"));

		}

		[Test()]
		public void TestIsTree()
		{
			g.IsDirected = false;
			g.AddVerticle("s");
			Assert.IsTrue(g.IsTree());
			g.AddVerticle("v1");
			Assert.IsFalse(g.IsTree());
			Assert.IsTrue(g.AddEdge("s","v1"));
			Assert.IsTrue(g.IsTree());
			Assert.IsTrue(g.AddEdge("v1","v2"));
			Assert.IsTrue(g.IsTree());
			Assert.IsTrue(g.AddEdge("s","u1"));
			Assert.IsTrue(g.IsTree());
			Assert.IsTrue(g.AddEdge("v2","v3"));
			Assert.IsTrue(g.IsTree());
			Assert.IsTrue(g.AddEdge("v3","s"));
			Assert.IsFalse(g.IsTree());
		}

		[Test()]
		public void TestDijkstra()
		{
			g.AddEdge("s","A",5);
			g.AddEdge("s","B",2);
			g.AddEdge("B","A",8);
			g.AddEdge("A","D",2);
			g.AddEdge("A","C",4);
			g.AddEdge("B","D",7);
			g.AddEdge("C","D",6);
			g.AddEdge("C","t",3);
			g.AddEdge("D","t",1);
			g.Dijkstra("s");
			Assert.AreEqual(0,g.GetVerticle("s").Dist);
			Assert.AreEqual(5,g.GetVerticle("A").Dist);
			Assert.AreEqual(2,g.GetVerticle("B").Dist);
			Assert.AreEqual(9,g.GetVerticle("C").Dist);
			Assert.AreEqual(7,g.GetVerticle("D").Dist);
			Assert.AreEqual(8,g.GetVerticle("t").Dist);
		}

		[Test()]
		public void TestBellmanFord()
        {
			g.AddEdge("A","B",-1);
			g.AddEdge("A","C",4);
			g.AddEdge("B","C",3);
			g.AddEdge("B","D",2);
			g.AddEdge("B","E",3);
			g.AddEdge("D","C",5);
			g.AddEdge("D","B",1);
			g.AddEdge("E","D",-3);
			g.BellmanFord("A");
			Assert.AreEqual(0,g.GetVerticle("A").Dist);
			Assert.AreEqual(-1,g.GetVerticle("B").Dist);
			Assert.AreEqual(2,g.GetVerticle("C").Dist);
			Assert.AreEqual(-1,g.GetVerticle("D").Dist);
			Assert.AreEqual(2,g.GetVerticle("E").Dist);
        }

		[Test()]
		public void TestHasNegativeCycle()
        {
			g.AddEdge("A","B",-1);
            g.AddEdge("A","C",4);
            g.AddEdge("B","C",3);
            g.AddEdge("B","D",2);
            g.AddEdge("B","E",3);
            g.AddEdge("D","C",5);
            g.AddEdge("D","B",1);
            g.AddEdge("E","D",-3);
			Assert.IsFalse(g.HasNegativeCycle());
			g.AddEdge("E","A",-3);
			Assert.IsTrue(g.HasNegativeCycle());
        }

		[Test()]
		public void TestGetShortestPath()
        {
			g.AddEdge("s","v1");
			g.AddEdge("s","v3");
			g.AddEdge("s","v4");
			g.AddEdge("v1","v2");
			g.AddEdge("v1","v4");
			g.AddEdge("v2","v5");
			g.AddEdge("v3","v4");
			g.AddEdge("v3","v6");
			g.AddEdge("v4","v5");
			g.AddEdge("v4","v7");
			g.AddEdge("v6","v4");
			g.AddEdge("v6","v7");
			g.AddEdge("v7","v5");
			g.AddEdge("v7","v8");
			g.AddEdge("s","v1");
			g.Bfs("s");
			Edge[] edges = g.GetShortestPath("s","v5").ToArray();
			Assert.AreEqual(2,edges.Length);
			Assert.AreSame(edges[0],g.GetEdge("s","v4"));
			Assert.AreSame(edges[1],g.GetEdge("v4","v5"));
        }
        

		[Test()]
		public void TestGetSubGraph()
        {
			g.AddEdge("s","v1");
            g.AddEdge("s","v3");
            g.AddEdge("s","v4");
            g.AddEdge("v1","v2");
            g.AddEdge("v1","v4");
            g.AddEdge("v2","v5");
            g.AddEdge("v3","v4");
            g.AddEdge("v3","v6");
            g.AddEdge("v4","v5");
            g.AddEdge("v4","v7");
            g.AddEdge("v6","v4");
            g.AddEdge("v6","v7");
            g.AddEdge("v7","v5");
            g.AddEdge("v7","v8");
            g.AddEdge("s","v1");
			var edges = new List<Edge>();
			edges.Add(g.GetEdge("s","v3"));
			edges.Add(g.GetEdge("v3","v6"));
			edges.Add(g.GetEdge("v7","v8"));
			Graph subg = g.GetSubGraph(edges.ToArray());
			Assert.IsTrue(subg.ContainsEdge("s","v3"));
			Assert.IsTrue(subg.ContainsEdge("v3","v6"));
			Assert.IsTrue(subg.ContainsEdge("v7","v8"));
			Assert.IsTrue(subg.ContainsVerticle("s"));
			Assert.IsTrue(subg.ContainsVerticle("v3"));
			Assert.IsTrue(subg.ContainsVerticle("v6"));
			Assert.IsTrue(subg.ContainsVerticle("v7"));
			Assert.IsTrue(subg.ContainsVerticle("v8"));
			Assert.AreEqual(5,subg.NumOfVerticles);
			Assert.AreEqual(3, subg.NumOfEdges);
        }

		[Test()]
		public void TestGetSubGraph2()
		{
			g.AddEdge("s","v1");
            g.AddEdge("s","v3");
            g.AddEdge("s","v4");
            g.AddEdge("v1","v2");
            g.AddEdge("v1","v4");
            g.AddEdge("v2","v5");
            g.AddEdge("v3","v4");
            g.AddEdge("v3","v6");
            g.AddEdge("v4","v5");
            g.AddEdge("v4","v7");
            g.AddEdge("v6","v4");
            g.AddEdge("v6","v7");
            g.AddEdge("v7","v5");
            g.AddEdge("v7","v8");
            g.AddEdge("s","v1");
			var verts = new List<Verticle>();
			verts.Add(g.GetVerticle("v4"));
			verts.Add(g.GetVerticle("v5"));
			verts.Add(g.GetVerticle("v7"));
			Graph subg = g.GetSubGraph(verts);
			Assert.IsTrue(subg.ContainsEdge("v4","v5"));
			Assert.IsTrue(subg.ContainsEdge("v4","v7"));
			Assert.IsTrue(subg.ContainsEdge("v7","v5"));
			Assert.AreEqual(3,subg.NumOfEdges);
			Assert.IsTrue(subg.ContainsVerticle("v4"));
			Assert.IsTrue(subg.ContainsVerticle("v5"));
			Assert.IsTrue(subg.ContainsVerticle("v7"));
			Assert.AreEqual(3,subg.NumOfVerticles);
		}
    }
}
