
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FCHA.DataTypes.Calendar
{
    [JsonObject(MemberSerialization.Fields)]
    public class JsonMonth
    {
        public Dictionary<string, JsonDay> Days;
    }
}