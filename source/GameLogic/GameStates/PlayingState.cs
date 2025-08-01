using GameLogic.Events.GameEvents;
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
            GameContext.EventManager.Subscribe<OnMilitaryTokenReachedThreshold>(OnMilitaryTokenReachedThreshold);
            GameContext.EventManager.Subscribe<MilitaryVictory>(OnScientificOrMilitaryVictory);
            GameContext.EventManager.Subscribe<ScientificVictory>(OnScientificOrMilitaryVictory);

            while (!IsGameOver)
            {
                IPlayerTurnState playerTurnState = new PickCardState(GameContext);

                while (playerTurnState is not EndTurn)
                {
                    playerTurnState.ExecuteTurnState();
                    playerTurnState = playerTurnState.GetNextTurnState();
                }

                if (!IsGameOver)
                {

                    if (GameContext.AgeHandler.CurrentAge.IsAgeOver)
                    {
                        IsGameOver = !GameContext.AgeHandler.NextAge();
                    }

                    GameContext.TurnHandler.NextPlayer();
                }
            }

            GameContext.EventManager.Unsubscribe<OnMilitaryTokenReachedThreshold>(OnMilitaryTokenReachedThreshold);
            GameContext.EventManager.Unsubscribe<MilitaryVictory>(OnScientificOrMilitaryVictory);
            GameContext.EventManager.Unsubscribe<ScientificVictory>(OnScientificOrMilitaryVictory);
            GameContext.EventManager.Publish(new OnGameEnded([GameContext.TurnHandler.CurrentPlayer, GameContext.TurnHandler.OpponentPlayer]));

        }

        public IGameState GetNextState()
        {
            return null;
        }

        private void OnMilitaryTokenReachedThreshold(OnMilitaryTokenReachedThreshold eventArgs)
        {
            eventArgs.MilitaryCards.ForEach(militaryCard => militaryCard.Apply(GameContext));
        }

        private void OnScientificOrMilitaryVictory(GameEvent args)
        {
            IsGameOver = true;
        }
    }
}
