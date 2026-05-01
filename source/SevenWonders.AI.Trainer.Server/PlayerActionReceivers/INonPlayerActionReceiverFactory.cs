using SevenWonders.Game.Logic.Interfaces;

namespace SevenWonders.AI.Trainer.Server.PlayerActionReceivers
{
    public interface INonPlayerActionReceiverFactory
    {
        IPlayerActionReceiver CreateNonPlayerActionReceiver(NonPlayerType nonPlayerType);
    }
}
