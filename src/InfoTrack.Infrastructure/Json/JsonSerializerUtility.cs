using System.Text.Json;
using System.Text.Json.Serialization;

namespace InfoTrack.Infrastructure.Json
{
    public static class JsonSerializerUtility
    {
        public static JsonSerializerOptions Settings { get; } = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };
    }
}
