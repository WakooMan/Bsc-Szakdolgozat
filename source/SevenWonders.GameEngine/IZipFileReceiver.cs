namespace SevenWonders.GameEngine
{
    public interface IZipFileReceiver
    {
        Task<ICollection<SceneFile>> ReceiveZipFiles();
    }
}