using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCHA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA.Tests
{
    [TestClass()]
    public class DepositTests
    {
        [TestMethod()]
        public void DepositTest()
        {
            Deposit d = new Deposit(100000m, 0.12, new DateTime(2015, 1, 1), 12);
            Assert.AreEqual(d.Duration, 12);
            Assert.IsTrue(Math.Abs(d.PrincipalAtMaturity-112682.5m) < 0.01m);
        }

        [TestMethod()]
        public void PrincipalAtTest()
        {
            Deposit d = new Deposit(100000m, 0.12, new DateTime(2015, 1, 1), 12);
            Assert.AreEqual(d.Duration, 12);
            Assert.IsTrue(Math.Abs(d.PrincipalAtMaturity - 112682.5m) < 0.01m);
            Assert.IsTrue(Math.Abs(d.PrincipalAt(new DateTime(2016, 1, 1)) - 112682.5m) < 0.01m);
            Assert.IsTrue(Math.Abs(d.PrincipalAt(new DateTime(2015, 2, 1)) - 101000.0m) < 0.01m);
        }

        [TestMethod()]
        public void InterestToDateTest()
        {
            Deposit d = new Deposit(100000m, 0.12, new DateTime(2015, 1, 1), 12);
            Assert.AreEqual(d.Duration, 12);
            Assert.IsTrue(Math.Abs(d.TotalInterest - 12682.5m) < 0.01m);
            Assert.IsTrue(Math.Abs(d.InterestToDate(new DateTime(2016, 1, 1)) - 12682.5m) < 0.01m);
            Assert.IsTrue(Math.Abs(d.InterestToDate(new DateTime(2015, 2, 1)) - 1000.0m) < 0.01m);
        }
    }
}