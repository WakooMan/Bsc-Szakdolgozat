using GameLogic.Events;
using GameLogic.Handlers;
using GameLogic.PlayerTurnStates;

namespace GameLogic.GameStates
{
    public class PlayingState : IGameState
    {
        public bool IsGameOver { get; private set; }
        public IGameContext GameContext { get; }

        public PlayingState(IGameContext gameContext)
        {
            IsGameOver = false;
            GameContext = gameContext;
        }
        
        public void DoStateAction()
        {
            while (!IsGameOver)
            {
                IPlayerTurnState playerTurnState = new PickCardState(GameContext);

                while (playerTurnState is not EndTurn)
                {
                    playerTurnState.ExecuteTurnState();
                    playerTurnState = playerTurnState.GetNextTurnState();
                }

                // Check If player won in the turn.

                if (GameContext.AgeHandler.CurrentAge.IsAgeOver)
                {
                    IsGameOver = !GameContext.AgeHandler.NextAge();
                }

                GameContext.TurnHandler.NextPlayer();
            }
        }

        public IGameState GetNextState()
        {
            return null;
        }
    }
}
