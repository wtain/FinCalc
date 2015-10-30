using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCHA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA.Tests
{
    [TestClass()]
    public class StringExtensionsTests
    {
        [TestMethod()]
        public void IsNullOrEmptyTest()
        {
            string nullString = null;
            Assert.IsTrue(nullString.IsNullOrEmpty());
            Assert.IsTrue(string.Empty.IsNullOrEmpty());
            Assert.IsTrue("".IsNullOrEmpty());
            Assert.IsFalse("aa".IsNullOrEmpty());
        }

        [TestMethod()]
        public void IsNullTest()
        {
            string nullString = null;
            Assert.IsTrue(nullString.IsNull());
            Assert.IsFalse(string.Empty.IsNull());
        }

        [TestMethod()]
        public void IsEmptyTest()
        {
            string nullString = null;
            Assert.IsFalse(nullString.IsEmpty());
            Assert.IsTrue(string.Empty.IsEmpty());
        }
    }
}