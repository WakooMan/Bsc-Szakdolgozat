using GameLogic.Elements;

namespace GameLogic.Events.GameEvents
{
    public class OnGameEnded: GameEvent
    {
        public (string name, int victoryPoints, int numberOfBlueCards) FirstPlayer { get; }
        public (string name, int victoryPoints, int numberOfBlueCards) SecondPlayer { get; }

        public OnGameEnded((string name, int victoryPoints, int numberOfBlueCards) firstPlayer, (string name, int victoryPoints, int numberOfBlueCards) secondPlayer)
        {
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;
        }
    }
}
