using System;
using Newtonsoft.Json;

namespace FLS.Common.Converters
{
    /// <summary>
    ///https://stackoverflow.com/questions/14524669/how-to-get-newtonsoft-to-deserialize-yes-and-no-to-boolean
    /// </summary>
    public class JsonBooleanConverter : JsonConverter
    {
        public override bool CanWrite { get { return false; } }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Boolean);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString().ToLower().Trim();
            switch (value)
            {
                case "true":
                case "yes":
                case "y":
                case "1":
                    return true;
            }
            return false;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
