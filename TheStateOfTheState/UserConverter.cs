using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TheStateOfTheState
{
    public class UserConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(User);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            User user = new User();

            // Populate the object properties
            user.Name = (string)jObject["name"];
            user.Mail = (string)jObject["mail"];
            user.Pwd = (string)jObject["pwd"];
            user.City = (string)jObject["city"];
            user.Age = (int)jObject["age"];
            user.Religion = (General.ReligionTypes)Enum.Parse(typeof(General.ReligionTypes), (string)jObject["religion"]);
            user.Orientation = (General.OrientationTypes)Enum.Parse(typeof(General.OrientationTypes), (string)jObject["orientation"]);
            user.Exist = (bool)jObject["exist"];

            return user;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Not needed for this example
            throw new NotImplementedException();
        }
    }

}