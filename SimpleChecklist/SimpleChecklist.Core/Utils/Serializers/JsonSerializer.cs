using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SimpleChecklist.Core.Utils.Serializers
{
    public static class JsonSerializer
    {
        public static string Serialize<T>(T obj)
        {
            var dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));

            string serializedData;
            using (var ms = new MemoryStream())
            {
                dataContractJsonSerializer.WriteObject(ms, obj);
                serializedData = Encoding.UTF8.GetString(ms.ToArray(), 0, (int) ms.Length);
            }

            return serializedData;
        }

        public static T Deserialize<T>(string data)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                return (T) ser.ReadObject(ms);
            }
        }
    }
}