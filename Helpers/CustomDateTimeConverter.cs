using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ang7.Helpers;


public class CustomDateTimeConverter : DateTimeConverterBase
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is DateTime dateTime)
        {
            writer.WriteValue(dateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        }
        else
        {
            writer.WriteNull();
        }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            string dateString = (string)reader.Value;
            if (dateString == "0000-00-00")
            {
                return DateTime.MinValue;
            }

            if (DateTime.TryParse(dateString, out DateTime parsedDate))
            {
                return parsedDate;
            }
        }
        return DateTime.MinValue; // or you could throw an exception or handle it differently
    }
}
