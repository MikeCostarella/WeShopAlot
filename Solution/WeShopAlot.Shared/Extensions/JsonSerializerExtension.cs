using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Text;
using WeShopAlot.Shared.Converter;
using WeShopAlot.Shared.Models;

namespace WeShopAlot.Shared.Extensions
{
    public static class JsonSerializerExtension
    {
        public static string SerializeObject(object value)
        {
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new ConstructorResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            };

            jsonSettings.Converters.Add(new StringEnumConverter
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            });

            return JsonConvert.SerializeObject(value, Formatting.None, jsonSettings);
        }

        public static T DeserializeObject<T>(string json, bool isTypeInt = false)
        {
            try
            {
                JsonSerializerSettings jsonSettings;
                if (isTypeInt)
                {
                    //add a new Json Int32 converter if input data type is int 
                    jsonSettings = new JsonSerializerSettings
                    {
                        Converters = new List<JsonConverter> { new JsonInt32Converter() }
                    };
                }
                else
                {
                    jsonSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new ConstructorResolver(),
                        NullValueHandling = NullValueHandling.Ignore,
                        TypeNameHandling = TypeNameHandling.None,
                        StringEscapeHandling = StringEscapeHandling.EscapeHtml
                    };

                }

                //Deserializing the response recieved from web api and storing into the Permit list    
                return JsonConvert.DeserializeObject<T>(json, jsonSettings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T DeserializeOKResponse<T>(string json)
        {
            if (typeof(T) == typeof(bool))
            {
                OkResponse response = DeserializeObject<OkResponse>(json);
                return (T)response.Result;
            }
            else if (typeof(T) == typeof(int))
            {
                OkResponse response = DeserializeObject<OkResponse>(json, true);
                return (T)response.Result;
            }
            else
            {
                OkResponse response = DeserializeObject<OkResponse>(json);
                return DeserializeObject<T>(response.Result.ToString());
            }
        }

        public static StringContent SerializeObjectToStringContent(object value)
        {
            var serializedValue = SerializeObject(value);
            var result = new StringContent(serializedValue, Encoding.UTF8, "application/json");
            return result;
        }

  
    }
}
