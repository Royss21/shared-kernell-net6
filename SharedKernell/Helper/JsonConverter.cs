
namespace SharedKernell.Helpers
{
    using System.Text;
    using System.Text.Json;
    public static class JsonConverter
    {
        public static StringContent SerializeContent(this object datos, string mediatype = "application/json", JsonSerializerOptions? options = null)
        {
            return new StringContent(
               JsonSerializer.Serialize(datos, options),
               Encoding.UTF8,
               mediatype);
        }

        public static T? Deserializer<T>(this object value)
        {
            return JsonSerializer.Deserialize<T>(value?.ToString() ?? "",new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
       
    }
}
