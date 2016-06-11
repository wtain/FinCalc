
using Newtonsoft.Json;

namespace FCHA.DataTypes.Calendar
{
    [JsonObject(MemberSerialization.Fields)]
    public class JsonDay
    {
        public int isWorking;
    }
}