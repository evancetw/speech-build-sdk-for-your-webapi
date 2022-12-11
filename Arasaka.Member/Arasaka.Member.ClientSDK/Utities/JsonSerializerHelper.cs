using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Arasaka.Member.ClientSDK.Utities
{
    static class JsonSerializerHelper
    {
        static JsonSerializerOptions _serializeOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        static JsonSerializerOptions _deserializeOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        internal static string Serialize(object value)
        {
            return JsonSerializer.Serialize(value, _serializeOptions);
        }

        internal static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _deserializeOptions);
        }

        internal static T Deserialize<T>(Stream utf8Json)
        {
            return JsonSerializer.Deserialize<T>(utf8Json, _deserializeOptions);
        }

        internal static async Task<T> DeserializeAsync<T>(Stream utf8Json)
        {
            return await JsonSerializer.DeserializeAsync<T>(utf8Json, _deserializeOptions);
        }


    }
}
