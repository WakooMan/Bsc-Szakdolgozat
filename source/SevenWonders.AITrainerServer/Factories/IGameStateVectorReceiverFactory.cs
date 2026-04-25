using SevenWonders.AI.Model.Services;

namespace SevenWonders.AITrainerServer.Factories
{
    public interface IGameStateVectorReceiverFactory
    {
        IGameStateVectorReceiver CreateEasy();
        IGameStateVectorReceiver CreateMedium();
    }
}
