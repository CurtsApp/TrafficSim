using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrafficSim.Roads;

namespace TrafficSim
{
    class TileConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            switch (jo["ClassName"].Value<string>())
            {
                //TODO Add cases for all possible roads
                case "TwoLaneRoad":
                    return jo.ToObject<TwoLaneRoad>(serializer);
                case "Intersection":
                    return jo.ToObject<Intersection>(serializer);
                case "Office":
                    return jo.ToObject<Office>(serializer);
                case "Home":
                    return jo.ToObject<Home>(serializer);
                case "Vacant":
                    return jo.ToObject<Vacant>(serializer);
                default:
                    throw new Exception(
                        "JSON Converter Type not found.");
            }

            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return(objectType == typeof(ITile));
        }
    }
}
