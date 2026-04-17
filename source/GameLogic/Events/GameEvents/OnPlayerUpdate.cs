using GameLogic.Elements;

namespace GameLogic.Events.GameEvents
{
    public class OnPlayerUpdate: GameEvent
    {
        public PlayerProperties Player1 { get; }
        public PlayerProperties Player2 { get; }

        public OnPlayerUpdate(PlayerProperties player1, PlayerProperties player2)
        {
            Player1 = player1;
            Player2 = player2;
        }
    }
}
