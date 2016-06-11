using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FCHA.DataTypes;

namespace FCHA
{
    public class CombinedCalendar : Calendar
    {
        private List<Calendar> calendars = new List<Calendar>();

        public CombinedCalendar()
        {

        }

        public CombinedCalendar(params Calendar[] calendarList)
        {
            calendars.AddRange(calendarList);
        }

        public void Add(Calendar calendar)
        {
            calendars.Add(calendar);
        }

        public override bool IsHoliday(DateTime date)
        {
            foreach (Calendar c in calendars)
                if (c.IsHoliday(date))
                    return true;
            return false;
        }
    }
}
