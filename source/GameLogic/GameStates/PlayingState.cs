using GameLogic.Events;
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
            GameContext.EventManager.Subscribe<OnMilitaryTokenReachedThreshold>(GameEventType.MilitaryTokenReachedThreshold, OnMilitaryTokenReachedThreshold);
            GameContext.EventManager.Subscribe<EventArgs>(GameEventType.MilitaryVictory, OnScientificOrMilitaryVictory);
            GameContext.EventManager.Subscribe<EventArgs>(GameEventType.ScientificVictory, OnScientificOrMilitaryVictory);

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

            GameContext.EventManager.Unsubscribe<OnMilitaryTokenReachedThreshold>(GameEventType.MilitaryTokenReachedThreshold, OnMilitaryTokenReachedThreshold);
            GameContext.EventManager.Unsubscribe<EventArgs>(GameEventType.MilitaryVictory, OnScientificOrMilitaryVictory);
            GameContext.EventManager.Unsubscribe<EventArgs>(GameEventType.ScientificVictory, OnScientificOrMilitaryVictory);
            GameContext.EventManager.Publish(GameEventType.GameEnded, new OnGameEnded([GameContext.TurnHandler.CurrentPlayer, GameContext.TurnHandler.OpponentPlayer]));

        }

        public IGameState GetNextState()
        {
            return null;
        }

        private void OnMilitaryTokenReachedThreshold(OnMilitaryTokenReachedThreshold eventArgs)
        {
            eventArgs.MilitaryCards.ForEach(militaryCard => militaryCard.Apply(GameContext));
        }

        private void OnScientificOrMilitaryVictory(EventArgs args)
        {
            IsGameOver = true;
        }
    }
}
