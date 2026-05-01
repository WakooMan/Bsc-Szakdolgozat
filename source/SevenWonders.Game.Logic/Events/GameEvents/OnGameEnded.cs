using SevenWonders.Game.Logic.Elements;

namespace SevenWonders.Game.Logic.Events.GameEvents
{
    public class OnGameEnded: GameEvent
    {
        public PlayerProperties FirstPlayer { get; }
        public PlayerProperties SecondPlayer { get; }

        public OnGameEnded(PlayerProperties firstPlayer, PlayerProperties secondPlayer)
        {
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;
        }
    }
}
