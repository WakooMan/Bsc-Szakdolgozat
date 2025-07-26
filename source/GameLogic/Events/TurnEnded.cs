using GameLogic.Elements;
using GameLogic.Handlers;

namespace GameLogic.Events
{
    public class TurnEnded: EventArgs
    {
        public ITurnHandler TurnHandler { get; }

        public TurnEnded(ITurnHandler turnHandler)
        {
            TurnHandler = turnHandler;
        }

    }
}
