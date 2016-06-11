

using System;
using FCHA.DataTypes;

namespace FCHA
{
    public class WeekendsCalendar : Calendar
    {
        public override bool IsHoliday(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday ||
                   date.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
