namespace SevenWonders.Presenter
{
    public interface IGameOverHandlerFactory
    {
        IGameOverHandler Create(bool isMultiplayer);
    }
}
