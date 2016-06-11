using FCHA.DataTypes;
using FCHA.DataTypes.Calendar;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FCHA
{
    public abstract class Calendar
    {
        public static SimpleCalendar FromJSon(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                JsonCalendar cal = JsonConvert.DeserializeObject<JsonCalendar>(sr.ReadToEnd());
                return new SimpleCalendar(cal);
            }
        }

        public static WeekendsCalendar Weekends
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public static readonly SimpleCalendar EmptyCalendar = new SimpleCalendar();

        public abstract bool IsHoliday(DateTime date);

        public DateTime BumpDate(DateTime date, BumpType bumpType)
        {
            throw new NotImplementedException();
        }

        public DateTime AddBusinessDays(int amount)
        {
            throw new NotImplementedException();
        }
    }
}
