
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FCHA.DataTypes.Calendar
{
    [JsonObject(MemberSerialization.Fields)]
    public class JsonYear
    {
        public Dictionary<string, JsonMonth> Months;
    }
}