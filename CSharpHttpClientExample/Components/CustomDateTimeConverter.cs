using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Commons.Components
{
    public class CustomDateTimeConverter : JsonConverter<DateTime?>
    {
        private readonly string dateFormat;
        public CustomDateTimeConverter(string _dateFormate)
        {
            this.dateFormat = _dateFormate;
        }
        public CustomDateTimeConverter()
        {
            this.dateFormat = "yyyy-MM-dd'T'HH:mm:ss";
        }

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (string.IsNullOrEmpty(reader.GetString()))
            {
                return null;
            }

            return DateTime.ParseExact(reader.GetString(), dateFormat, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                return;
            }
            writer.WriteStringValue(value.Value.ToString(dateFormat, CultureInfo.InvariantCulture));
        }
    }

    public class CustomDateTimeConverter2 : JsonConverter<DateTime>
    {
        private readonly string dateFormat;
        public CustomDateTimeConverter2(string _dateFormate)
        {
            this.dateFormat = _dateFormate;
        }
        public CustomDateTimeConverter2()
        {
            this.dateFormat = "yyyy-MM-dd'T'HH:mm:ss";
        }
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString(), dateFormat, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(dateFormat, CultureInfo.InvariantCulture));
        }
    }
}