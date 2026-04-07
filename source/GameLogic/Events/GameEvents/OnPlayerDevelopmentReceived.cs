using GameLogic.Elements;
using GameLogic.Elements.Modifiers;

namespace GameLogic.Events.GameEvents
{
    public class OnPlayerDevelopmentReceived: GameEvent
    {
        public Development Development { get; }
        public Player Player { get; }

        public OnPlayerDevelopmentReceived(Player player, Development development)
        {
            Player = player;
            Development = development;
        }
    }
}
