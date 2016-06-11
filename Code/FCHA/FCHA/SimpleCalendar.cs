using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FCHA.DataTypes;
using FCHA.DataTypes.Calendar;

namespace FCHA
{
    public class SimpleCalendar : Calendar
    {
        private HashSet<DateTime> m_holidays = new HashSet<DateTime>();

        public SimpleCalendar()
        {            
        }

        internal SimpleCalendar(JsonCalendar cal)
        {
            foreach (string y in cal.data.Keys)
            {
                int year = int.Parse(y);
                foreach (string m in cal.data[y].Keys)
                {
                    int month = int.Parse(m);
                    foreach (string d in cal.data[y][m].Keys)
                    {
                        if (3 == cal.data[y][m][d].isWorking)
                            continue;
                        int day = int.Parse(d);
                        m_holidays.Add(new DateTime(year, month, day));
                    }
                }
            }
        }

        public override bool IsHoliday(DateTime date)
        {
            return m_holidays.Contains(date.Date);
        }
    }
}
