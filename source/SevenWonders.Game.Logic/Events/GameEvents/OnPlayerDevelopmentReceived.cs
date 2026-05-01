using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Modifiers;

namespace SevenWonders.Game.Logic.Events.GameEvents
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
