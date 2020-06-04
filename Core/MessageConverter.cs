using Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Core
{
    public class MessageConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IMessage));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            if (jo["Type"].Value<int>() == 0)
                return jo.ToObject<AudioMessage>(serializer);
            else if (jo["Type"].Value<int>() == 1)
                return jo.ToObject<LogMessage>(serializer);
            return null;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public static string CreateJson(IMessage message, Dictionary<string, string> dictionary)
        {
            message.Payload = dictionary;      
            string json = JsonConvert.SerializeObject(message);
            return json;
        }
    }
}
