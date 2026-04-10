namespace SevenWonders.Common
{
    public interface IXmlHandler
    {
        void SerializeFile<T>(string filePath, T obj);
        T DeserializeFile<T>(string filePath);
        void SerializeEmbeddedResource<T>(string resourcePath, T obj);
        T DeserializeEmbeddedResource<T>(string resourcePath);
    }
}
