using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Events.GameEvents;
using GameLogic.Handlers;
using GameLogic.PlayerTurnStates;
using SevenWonders.Common;
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
        
        public void DoStateAction()
        {
            GameLog.Info("DoStateAction started.");
            m_gameOverType = typeof(OnGameEnded);
            GameContext.EventManager.Subscribe<MilitaryVictory>(OnScientificOrMilitaryVictory);
            GameContext.EventManager.Subscribe<ScientificVictory>(OnScientificOrMilitaryVictory);
            GameContext.AgeHandler.Initialize(GameContext.RandomGenerator);

            while (!IsGameOver)
            {
                GameLog.Info($"Turn starting for player: {GameContext.TurnHandler.CurrentPlayer.Name} (Id={GameContext.TurnHandler.CurrentPlayer.Id})");
                GameContext.EventManager.Publish(new TurnStarted(GameContext.TurnHandler.CurrentPlayer));
                IPlayerTurnState playerTurnState = new PickCardState(GameContext);

                while (playerTurnState is not EndTurn)
                {
                    playerTurnState.ExecuteTurnState();
                    playerTurnState = playerTurnState.GetNextTurnState();
                }

                Player firstPlayer = GameContext.TurnHandler.GetPlayer(1);
                Player secondPlayer = GameContext.TurnHandler.GetPlayer(2);
                PlayerProperties firstPlayerProperties = firstPlayer.GetPlayerProperties(secondPlayer);
                PlayerProperties secondPlayerProperties = secondPlayer.GetPlayerProperties(firstPlayer);
                GameContext.MilitaryBoard.OnUpdate(GameContext, firstPlayerProperties, secondPlayerProperties);

                PlayerProperties firstPlayerProperties2 = firstPlayer.GetPlayerProperties(secondPlayer);
                PlayerProperties secondPlayerProperties2 = secondPlayer.GetPlayerProperties(firstPlayer);
                GameContext.EventManager.Publish(new OnPlayerUpdate(firstPlayerProperties2, secondPlayerProperties2));

                if (!IsGameOver)
                {

                    if (GameContext.AgeHandler.CurrentAge.IsAgeOver)
                    {
                        GameLog.Info($"Age {GameContext.AgeHandler.CurrentAge.Age} is over. Attempting next age...");
                        IsGameOver = !GameContext.AgeHandler.NextAge();
                        if (IsGameOver)
                        {
                            GameLog.Info("No more ages. Game over.");
                        }
                    }

                    GameContext.TurnHandler.NextPlayer();
                }
            }

            GameContext.EventManager.Unsubscribe<MilitaryVictory>(OnScientificOrMilitaryVictory);
            GameContext.EventManager.Unsubscribe<ScientificVictory>(OnScientificOrMilitaryVictory);
            if (m_gameOverType == typeof(OnGameEnded))
            {
                GameLog.Info("Game ended normally. Publishing OnGameEnded.");
                Player firstPlayer = GameContext.TurnHandler.GetPlayer(1);
                Player secondPlayer = GameContext.TurnHandler.GetPlayer(2);
                PlayerProperties firstPlayerProperties = firstPlayer.GetPlayerProperties(secondPlayer);
                PlayerProperties secondPlayerProperties = secondPlayer.GetPlayerProperties(firstPlayer);
                GameLog.Info($"Final scores: {firstPlayer.Name} VP={firstPlayerProperties.VictoryPoints}, {secondPlayer.Name} VP={secondPlayerProperties.VictoryPoints}");
                GameContext.EventManager.Publish(new OnGameEnded(firstPlayerProperties, secondPlayerProperties));
            }
            else
            {
                GameLog.Info($"Game ended by {m_gameOverType.Name}.");
            }
        }

        public IGameState GetNextState()
        {
            return new EndGameState();
        }

        private void OnScientificOrMilitaryVictory(GameEvent args)
        {
            GameLog.Info($"Instant victory triggered: {args.GetType().Name}");
            m_gameOverType = args.GetType();
            IsGameOver = true;
        }

        private Type m_gameOverType;
    }
}
