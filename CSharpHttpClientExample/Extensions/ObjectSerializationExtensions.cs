using Commons.Components;

namespace Commons.Extensions
{
    public static class ObjectSerializationExtensions
    {
        private static readonly string DATE_FORMAT = "yyyy-MM-dd'T'HH:mm:ss";

        public static string ToJsonForEnumString(this object obj)
        {
            System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions
            {
                Converters ={
                    new System.Text.Json.Serialization.JsonStringEnumConverter(),
                    new CustomDateTimeConverter(),
                    new CustomDateTimeConverter2()
                }
            };

            return System.Text.Json.JsonSerializer.Serialize(obj, options);
        }

        public static object FromJsonForEnumString(this string obj, System.Type returnType)
        {
            System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions
            {
                Converters ={
                    new System.Text.Json.Serialization.JsonStringEnumConverter(),
                    new CustomDateTimeConverter(),
                    new CustomDateTimeConverter2()
                }
            };

            return System.Text.Json.JsonSerializer.Deserialize(obj, returnType, options);
        }

        public static T FromJsonForEnumString<T>(this string obj)
        {
            System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions
            {
                Converters ={
                    new System.Text.Json.Serialization.JsonStringEnumConverter(),
                    new CustomDateTimeConverter(),
                    new CustomDateTimeConverter2()
                }
            };

            return System.Text.Json.JsonSerializer.Deserialize<T>(obj, options);
        }

        public static byte[] SerializeObject(this object? obj)
        {
            if (obj == null)
                return new byte[0];
            string jsonValue = obj.ToJsonForEnumString();
            return System.Text.Encoding.UTF8.GetBytes(jsonValue);
        }

        public static T DeserializeObject<T>(this byte[] arrBytes)
        {
            if (arrBytes == null) { return default(T); }
            string jsonValue = System.Text.Encoding.UTF8.GetString(arrBytes);
            return jsonValue.FromJsonForEnumString<T>();
        }

        public static object? DeserializeObject(this byte[] arrBytes)
        {
            if (arrBytes == null) { return null; }
            string jsonValue = System.Text.Encoding.UTF8.GetString(arrBytes);
            return jsonValue.FromJsonForEnumString(typeof(object));
        }
    }
}
