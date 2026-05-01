namespace SevenWonders.Game.Engine
{
    public interface IZipFileReceiver
    {
        Task<ICollection<SceneFile>> ReceiveZipFiles();
    }
}