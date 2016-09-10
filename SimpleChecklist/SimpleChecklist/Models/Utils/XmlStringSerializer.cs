using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace SimpleChecklist.Models.Utils
{
    public class XmlStringSerializer
    {
        public static string Serialize<T>(T obj)
        {
            using (var memoryStream = new MemoryStream())
            using (var reader = new StreamReader(memoryStream))
            {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(memoryStream, obj);
                memoryStream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public static T Deserialize<T>(string data)
        {
            using (Stream stream = new MemoryStream())
            {
                var buffer = Encoding.UTF8.GetBytes(data);
                stream.Write(buffer, 0, buffer.Length);
                stream.Position = 0;
                var deserializer = new DataContractSerializer(typeof(T));
                return (T) deserializer.ReadObject(stream);
            }
        }
    }
}