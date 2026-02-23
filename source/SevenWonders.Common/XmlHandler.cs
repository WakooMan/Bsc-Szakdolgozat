using System.Xml.Serialization;

namespace SevenWonders.Common
{
    public class XmlHandler : IXmlHandler
    {
        public T Deserialize<T>(string filePath)
        {
            T obj = default(T);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                obj = (T)serializer.Deserialize(fs);
            }

            return obj;
        }

        public void Serialize<T>(string filePath, T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, obj);
            }
        }
    }
}
