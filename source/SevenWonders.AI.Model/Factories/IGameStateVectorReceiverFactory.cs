using SevenWonders.AI.Model.Services;

namespace SevenWonders.AI.Model.Factories
{
    public interface IGameStateVectorReceiverFactory
    {
        IGameStateVectorReceiver CreateEasy();
        IGameStateVectorReceiver CreateMedium();
        IGameStateVectorReceiver CreateHard();
    }
}
