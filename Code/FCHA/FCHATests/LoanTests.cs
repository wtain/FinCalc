using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCHA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA.Tests
{
    [TestClass()]
    public class LoanTests
    {
        public const decimal Eps = 1e-6m;

        [TestMethod()]
        public void LoanTest()
        {
            Loan l = new Loan(3800000m, 0.12, new DateTime(2015, 1, 1), 120, Calendar.EmptyCalendar);
            Assert.AreEqual(l.NumberOfPayments, 120);
            Assert.IsTrue(Math.Abs(l.TotalPrincipalPayment-3800000.0m) < Eps);
            Assert.IsTrue(Math.Abs(l.TotalPayment - l.TotalPrincipalPayment - l.TotalInterest) < Eps);

            Loan l1 = new Loan(521450.0m, 0.28, new DateTime(2015, 1, 25), 26, Calendar.EmptyCalendar); // RUS
            Assert.AreEqual(l1.NumberOfPayments, 26);
            Assert.IsTrue(Math.Abs(l1.TotalPrincipalPayment - 521450.0m) < Eps);
            Assert.IsTrue(Math.Abs(l1.TotalPayment - l1.TotalPrincipalPayment - l1.TotalInterest) < Eps);
        }

        [TestMethod()]
        public void AdditionalPaymentTest()
        {
            Loan l = new Loan(3800000m, 0.12, new DateTime(2015, 1, 1), 120, Calendar.EmptyCalendar);
            Assert.AreEqual(l.NumberOfPayments, 120);
            Assert.IsTrue(Math.Abs(l.TotalPrincipalPayment - 3800000.0m) < Eps);
            Assert.IsTrue(Math.Abs(l.TotalPayment - l.TotalPrincipalPayment - l.TotalInterest) < Eps);

            l.AdditionalPayment(new DateTime(2015, 6, 1), 200000);
            Assert.AreEqual(l.NumberOfPayments, 121);
            Assert.IsTrue(Math.Abs(l.TotalPrincipalPayment - 3800000.0m) < Eps);
            Assert.IsTrue(Math.Abs(l.TotalPayment - l.TotalPrincipalPayment - l.TotalInterest) < Eps);
        }
    }
}