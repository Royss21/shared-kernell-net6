
namespace SharedKernell.JsonConvert
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    public class TrimStringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TrimInputField(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }

        public string TrimInputField(string input)
        {
            if (!string.IsNullOrEmpty(input))
                input = input.Trim();

            return input;
        }
    }
}