using GameLogic.Interfaces;

namespace SevenWonders.AITrainerServer.PlayerActionReceivers
{
    public interface INonPlayerActionReceiverFactory
    {
        IPlayerActionReceiver CreateNonPlayerActionReceiver(NonPlayerType nonPlayerType);
    }
}
