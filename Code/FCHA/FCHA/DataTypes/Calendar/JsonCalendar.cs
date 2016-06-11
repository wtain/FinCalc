
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FCHA.DataTypes.Calendar
{
    [JsonObject(MemberSerialization.Fields)]
    public class JsonCalendar
    {
        public Dictionary<string, Dictionary<string, Dictionary<string, JsonDay>>> data;
    }
}