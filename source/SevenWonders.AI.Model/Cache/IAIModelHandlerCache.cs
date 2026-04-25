using SevenWonders.AI.Model.AIModelHandler;

namespace SevenWonders.AI.Model.Cache
{
    public interface IAIModelHandlerCache
    {
        IAIModelHandler EasyAIModelHandler { get; }
        IAIModelHandler MediumAIModelHandler { get; }
    }
}
