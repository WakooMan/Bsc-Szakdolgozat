using GameLogic.Elements;
using GameLogic.Handlers;
using GameLogic.Interfaces;

namespace GameLogic.Events
{
    public class TurnStarted : EventArgs
    {
        public IPlayerActionReceiver PlayerActionReceiver { get; }
        public ITurnHandler TurnHandler { get; }
        public TurnStarted(IPlayerActionReceiver playerActionReceiver, ITurnHandler turnHandler)
        {
            PlayerActionReceiver = playerActionReceiver;
            TurnHandler = turnHandler;
        }
    }
}
