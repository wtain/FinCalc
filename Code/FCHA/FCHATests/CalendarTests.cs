using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCHA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA.Tests
{
    [TestClass()]
    public class CalendarTests
    {
        [TestMethod()]
        public void FromJSonTest()
        {
            SimpleCalendar rus = Calendar.FromJSon("..\\..\\..\\..\\..\\Data\\RUS.json"); // up to the end of 2015
            Assert.IsTrue(rus.IsHoliday(new DateTime(2003, 1, 1)));
            Assert.IsTrue(rus.IsHoliday(new DateTime(2003, 1, 1, 12, 35, 50)));
            Assert.IsTrue(rus.IsHoliday(new DateTime(2015, 5, 1)));
        }
    }
}