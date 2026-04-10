using GameLogic.Events.GameEvents;
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
        
        public async Task DoStateAction()
        {
            GameContext.EventManager.Subscribe<MilitaryVictory>(OnScientificOrMilitaryVictory);
            GameContext.EventManager.Subscribe<ScientificVictory>(OnScientificOrMilitaryVictory);
            await GameContext.AgeHandler.Initialize();

            while (!IsGameOver)
            {
                await GameContext.EventManager.PublishAsync(new TurnStarted(GameContext.TurnHandler.CurrentPlayer));
                IPlayerTurnState playerTurnState = new PickCardState(GameContext);

                while (playerTurnState is not EndTurn)
                {
                    await playerTurnState.ExecuteTurnState();
                    playerTurnState = playerTurnState.GetNextTurnState();
                }

                if (!IsGameOver)
                {

                    if (GameContext.AgeHandler.CurrentAge.IsAgeOver)
                    {
                        IsGameOver = !await GameContext.AgeHandler.NextAge();
                    }

                    await GameContext.TurnHandler.NextPlayer();
                }
            }

            GameContext.EventManager.Unsubscribe<MilitaryVictory>(OnScientificOrMilitaryVictory);
            GameContext.EventManager.Unsubscribe<ScientificVictory>(OnScientificOrMilitaryVictory);
            await GameContext.EventManager.PublishAsync(new OnGameEnded([GameContext.TurnHandler.CurrentPlayer, GameContext.TurnHandler.OpponentPlayer]));

        }

        public IGameState GetNextState()
        {
            return new EndGameState();
        }

        private void OnScientificOrMilitaryVictory(GameEvent args)
        {
            IsGameOver = true;
        }
    }
}
