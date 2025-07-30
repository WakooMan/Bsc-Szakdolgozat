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
            GameContext.EventManager.Subscribe(GameEventType.MilitaryTokenReachedThreshold, OnMilitaryTokenReachedThreshold);
            GameContext.EventManager.Subscribe(GameEventType.MilitaryVictory, OnScientificOrMilitaryVictory);
            GameContext.EventManager.Subscribe(GameEventType.ScientificVictory, OnScientificOrMilitaryVictory);

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

            GameContext.EventManager.Unsubscribe(GameEventType.MilitaryTokenReachedThreshold, OnMilitaryTokenReachedThreshold);
            GameContext.EventManager.Unsubscribe(GameEventType.MilitaryVictory, OnScientificOrMilitaryVictory);
            GameContext.EventManager.Unsubscribe(GameEventType.ScientificVictory, OnScientificOrMilitaryVictory);
            GameContext.EventManager.Publish(GameEventType.GameEnded, new OnGameEnded([GameContext.TurnHandler.CurrentPlayer, GameContext.TurnHandler.OpponentPlayer]));

        }

        public IGameState GetNextState()
        {
            return null;
        }

        private void OnMilitaryTokenReachedThreshold(EventArgs args)
        {
            if (args is OnMilitaryTokenReachedThreshold eventArgs)
            {
                eventArgs.MilitaryCards.ForEach(militaryCard => militaryCard.Apply(GameContext));
            }
        }

        private void OnScientificOrMilitaryVictory(EventArgs args)
        {
            IsGameOver = true;
        }
    }
}
