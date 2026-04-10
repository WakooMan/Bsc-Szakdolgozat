using System.Xml.Serialization;

namespace SevenWonders.Common
{
    public class XmlHandler : IXmlHandler
    {

        public void SerializeFile<T>(string filePath, T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, obj);
            }
        }

        public T DeserializeFile<T>(string filePath)
        {
            T obj = default(T);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                obj = (T)serializer.Deserialize(fs);
            }

            return obj;
        }

        public T DeserializeEmbeddedResource<T>(string resourcePath)
        {
            T obj = default(T);
            var assembly = typeof(T).Assembly;
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (Stream? stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream is not null)
                {
                    obj = (T)serializer.Deserialize(stream);
                }
            }

            return obj;
        }

        public void SerializeEmbeddedResource<T>(string resourcePath, T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            var assembly = typeof(T).Assembly;

            using (Stream? stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream is not null)
                {
                    serializer.Serialize(stream, obj);
                }
            }
        }
    }
}
