using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ThtSizzlingHotProduct.Infrastructure.Persistence.Converters;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private const string Format = "dd/MM/yyyy";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();
        return DateTime.ParseExact(dateString!, Format, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}