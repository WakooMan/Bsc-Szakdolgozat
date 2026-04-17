using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Events.GameEvents;
using GameLogic.Handlers;
using GameLogic.PlayerTurnStates;
using System.Numerics;

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
            m_gameOverType = typeof(OnGameEnded);
        }
        
        public async Task DoStateAction()
        {
            m_gameOverType = typeof(OnGameEnded);
            GameContext.EventManager.Subscribe<MilitaryVictory>(OnScientificOrMilitaryVictory);
            GameContext.EventManager.Subscribe<ScientificVictory>(OnScientificOrMilitaryVictory);
            await GameContext.AgeHandler.Initialize(GameContext.RandomGenerator);

            while (!IsGameOver)
            {
                await GameContext.EventManager.PublishAsync(new TurnStarted(GameContext.TurnHandler.CurrentPlayer));
                IPlayerTurnState playerTurnState = new PickCardState(GameContext);

                while (playerTurnState is not EndTurn)
                {
                    await playerTurnState.ExecuteTurnState();
                    playerTurnState = playerTurnState.GetNextTurnState();
                }

                Player firstPlayer = GameContext.TurnHandler.GetPlayer(1);
                Player secondPlayer = GameContext.TurnHandler.GetPlayer(2);
                PlayerProperties firstPlayerProperties = await firstPlayer.GetPlayerProperties();
                PlayerProperties secondPlayerProperties = await secondPlayer.GetPlayerProperties();
                await GameContext.MilitaryBoard.OnUpdate(GameContext, firstPlayerProperties, secondPlayerProperties);

                PlayerProperties firstPlayerProperties2 = await firstPlayer.GetPlayerProperties();
                PlayerProperties secondPlayerProperties2 = await secondPlayer.GetPlayerProperties();
                await GameContext.EventManager.PublishAsync(new OnPlayerUpdate(firstPlayerProperties2, secondPlayerProperties2));

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
            if (m_gameOverType == typeof(OnGameEnded))
            {
                Player firstPlayer = GameContext.TurnHandler.GetPlayer(1);
                Player secondPlayer = GameContext.TurnHandler.GetPlayer(2);
                await firstPlayer.OnBeforeGameEnded(secondPlayer);
                await secondPlayer.OnBeforeGameEnded(firstPlayer);
                PlayerProperties firstPlayerProperties = await firstPlayer.GetPlayerProperties();
                PlayerProperties secondPlayerProperties = await secondPlayer.GetPlayerProperties();
                await GameContext.EventManager.PublishAsync(new OnGameEnded((firstPlayer.Name, firstPlayerProperties.VictoryPoints, firstPlayer.Cards.OfType<BlueCard>().Count()), (secondPlayer.Name, secondPlayerProperties.VictoryPoints, secondPlayer.Cards.OfType<BlueCard>().Count())));
            }
        }

        public IGameState GetNextState()
        {
            return new EndGameState();
        }

        private void OnScientificOrMilitaryVictory(GameEvent args)
        {
            m_gameOverType = args.GetType();
            IsGameOver = true;
        }

        private Type m_gameOverType;
    }
}
