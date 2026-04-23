using SevenWonders.AI.Model.AIModelHandler;

namespace SevenWonders.AITrainerServer.Cache
{
    public interface IAIModelHandlerCache
    {
        IAIModelHandler AIModelHandler { get; }
    }
}
