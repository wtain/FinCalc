using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCHA.WPFHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA.WPFHelpers.Tests
{
    [TestClass()]
    public class IEnumerableExtensionsTests
    {
        public class A
        {
            private bool visited = false;
            public A()
            {

            }

            public void Visit()
            {
                visited = true;
            }

            public bool Visited { get { return visited; } }
        }

        [TestMethod()]
        public void ForEachTest()
        {
            int N = 10;
            List<A> items = new List<A>(N);
            for (int i = 0; i < N; ++i)
                items.Add(new A());
            Assert.AreEqual(0, items.Count(a => a.Visited));
            items.ForEach(a => a.Visit());
            Assert.AreEqual(N, items.Count(a => a.Visited));
        }
    }
}