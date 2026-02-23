namespace SevenWonders.GameEngine
{
    public class SceneFile
    {
        public SceneFile(string name, Stream stream)
        {
            Name = name;
            Stream = stream;
        }

        public SceneFile()
        {
            Name = string.Empty;
            Stream = Stream.Null;
        }  

        public string Name { get; set; }

        public Stream Stream { get; set; }
    }
}
