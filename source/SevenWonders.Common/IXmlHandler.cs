namespace SevenWonders.Common
{
    public interface IXmlHandler
    {
        void Serialize<T>(string filePath, T obj);
        T Deserialize<T>(string filePath);
    }
}
