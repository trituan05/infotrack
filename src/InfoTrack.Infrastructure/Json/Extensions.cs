using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InfoTrack.Infrastructure.Json
{
    public static class Extensions
    {
        public static IMvcBuilder ConfigureJsonOptions(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddJsonOptions(json =>
            {
                json.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                json.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                json.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            return mvcBuilder;
        }

        public static bool TryParseJson<TOutput>(this string data, out TOutput? output)
        {
            try
            {
                output = JsonSerializer.Deserialize<TOutput>(data, JsonSerializerUtility.Settings);
                return true;
            }
            catch
            {
                output = default;
                return false;
            }
        }
    }


}
