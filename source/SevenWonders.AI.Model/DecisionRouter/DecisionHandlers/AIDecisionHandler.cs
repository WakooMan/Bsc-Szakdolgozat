using GameLogic;
using GameLogic.Elements;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using SevenWonders.AI.Model.Messages;
using SevenWonders.AI.Model.Services;
using SevenWonders.Common;

namespace SevenWonders.AI.Model.DecisionRouter.DecisionHandlers
{
    public class AIDecisionHandler : IAIDecisionHandler, IDisposable
    {
        public event Action<GameStateResponse>? OnGameStateReceived;

        public AIDecisionHandler(IGame game,
                                 IGameStateVectorReceiver gameStateVectorReceiver,
                                 IPlayerActionMaskReceiver playerActionMaskReceiver,
                                 IRewardCalculator rewardCalculator)
        {
            m_game = game;
            m_gameStateVectorReceiver = gameStateVectorReceiver;
            m_playerActionMaskReceiver = playerActionMaskReceiver;
            m_rewardCalculator = rewardCalculator;
            OnGameStateReceived = null;
            m_actionRequest = null;
        }

        public void Initialize()
        {
            GameLog.Info("Initializing...");
            m_gameStateVectorReceiver.Initialize();
            m_rewardCalculator.Reset();
            m_game.Context.EventManager.Subscribe<OnGameEnded>(GameEnded);
            m_game.Context.EventManager.Subscribe<MilitaryVictory>(OnMilitaryVictory);
            m_game.Context.EventManager.Subscribe<ScientificVictory>(OnScientificVictory);
            GameLog.Info("Initialized and subscribed to events.");
        }

        public void Uninitialize()
        {
            GameLog.Info("Uninitializing and unsubscribing from events...");
            m_game.Context.EventManager.Unsubscribe<OnGameEnded>(GameEnded);
            m_game.Context.EventManager.Unsubscribe<MilitaryVictory>(OnMilitaryVictory);
            m_game.Context.EventManager.Unsubscribe<ScientificVictory>(OnScientificVictory);
        }

        private void OnMilitaryVictory(MilitaryVictory victory)
        {
            GameLog.Info($"Military victory detected! Player={victory.PlayerProperties.Owner.Name}");
            PlayerProperties playerProperties = victory.PlayerProperties;
            PlayerProperties opponentProperties = victory.PlayerProperties.Opponent.GetPlayerProperties(victory.PlayerProperties.Owner);
            PhaseIndicator phase = PhaseIndicator.ChooseAction;
            List<float> stateVector = m_gameStateVectorReceiver.Receive(playerProperties, opponentProperties, phase);
            List<int> actionMask = m_playerActionMaskReceiver.ReceiveEmptyPlayerActionMask();
            GameStateResponse messageObj = new GameStateResponse
            {
                State = stateVector,
                Mask = actionMask,
                Reward = m_rewardCalculator.CalculateInstantWinReward(playerProperties),
                Terminated = true
            };
            OnGameStateReceived?.Invoke(messageObj);
        }

        private void OnScientificVictory(ScientificVictory victory)
        {
            GameLog.Info($"Scientific victory detected! Player={victory.PlayerProperties.Owner.Name}");
            PlayerProperties playerProperties = victory.PlayerProperties;
            PlayerProperties opponentProperties = victory.PlayerProperties.Opponent.GetPlayerProperties(victory.PlayerProperties.Owner);
            PhaseIndicator phase = PhaseIndicator.ChooseAction;
            List<float> stateVector = m_gameStateVectorReceiver.Receive(playerProperties, opponentProperties, phase);
            List<int> actionMask = m_playerActionMaskReceiver.ReceiveEmptyPlayerActionMask();
            GameStateResponse messageObj = new GameStateResponse
            {
                State = stateVector,
                Mask = actionMask,
                Reward = m_rewardCalculator.CalculateInstantWinReward(playerProperties),
                Terminated = true
            };
            OnGameStateReceived?.Invoke(messageObj);
        }

        private void GameEnded(OnGameEnded ended)
        {
            GameLog.Info($"Game ended! Player1={ended.FirstPlayer.Owner.Name} VP={ended.FirstPlayer.VictoryPoints}, Player2={ended.SecondPlayer.Owner.Name} VP={ended.SecondPlayer.VictoryPoints}");
            PlayerProperties playerProperties = ended.FirstPlayer;
            PlayerProperties opponentProperties = ended.SecondPlayer;
            PhaseIndicator phase = PhaseIndicator.ChooseAction;
            List<float> stateVector = m_gameStateVectorReceiver.Receive(playerProperties, opponentProperties, phase);
            List<int> actionMask = m_playerActionMaskReceiver.ReceiveEmptyPlayerActionMask();
            GameStateResponse messageObj = new GameStateResponse
            {
                State = stateVector,
                Mask = actionMask,
                Reward = m_rewardCalculator.CalculateVictoryPointsReward(playerProperties, opponentProperties),
                Terminated = true
            };
            OnGameStateReceived?.Invoke(messageObj);
        }

        public void Decide(ActionRequest actionRequest)
        {
            GameLog.Info($"Decide called with Action={actionRequest.Action}");
            m_actionRequest = actionRequest;
            m_actionReady.Set();
        }

        public PlayerActionWrapper HandleDecisions(Player player, ICollection<PlayerActionWrapper> playerActions)
        {
            GameLog.Info($"HandleDecisions: Player={player.Name}, ActionCount={playerActions.Count}");
            m_actionRequest = null;
            PlayerActionWrapper[] actionsArray = playerActions.ToArray();
            PhaseIndicator phase = DeterminePhase(actionsArray);
            GameLog.Info($"Phase={phase}");

            Player opponent = m_game.Players.First(p => p.Id != player.Id);
            PlayerProperties playerProperties = player.GetPlayerProperties(opponent);
            PlayerProperties opponentProperties = opponent.GetPlayerProperties(player);

            List<float> stateVector = m_gameStateVectorReceiver.Receive(playerProperties, opponentProperties, phase);
            List<int> actionMask = m_playerActionMaskReceiver.ReceivePlayerActionMask(phase, actionsArray);

            GameStateResponse messageObj = new GameStateResponse
            {
                State = stateVector,
                Mask = actionMask,
                Reward = m_rewardCalculator.CalculateTurnReward(playerProperties, opponentProperties),
                Terminated = false
            };

            GameLog.Info($"Sending game state. Reward={messageObj.Reward}, Terminated={messageObj.Terminated}");
            OnGameStateReceived?.Invoke(messageObj);

            GameLog.Info("Waiting for AI action...");
            m_actionReady.Wait();
            m_actionReady.Reset();
            GameLog.Info($"AI action received: {m_actionRequest?.Action}");

            var result = MapActionToWrapper(m_actionRequest, phase, actionsArray);
            GameLog.Info($"Mapped to action: {result.PlayerAction.GetType().Name}, CanPerform={result.CanPerform}");
            return result;
        }

        public void Dispose()
        {
            m_actionReady?.Dispose();
        }

        private static PhaseIndicator DeterminePhase(PlayerActionWrapper[] playerActions)
        {
            if (playerActions.Length > 0 && playerActions[0].PlayerAction is PickCard)
            {
                return PhaseIndicator.ChooseCard;
            }
            return PhaseIndicator.ChooseAction;
        }

        private PlayerActionWrapper MapActionToWrapper(ActionRequest actionRequest, PhaseIndicator phase, PlayerActionWrapper[] playerActions)
        {
            if (phase == PhaseIndicator.ChooseCard)
            {
                var allCards = m_game.Context.AgeHandler.CurrentAge.Composition.AllCards;
                ICardNode? targetNode = actionRequest.Action >= 0 && actionRequest.Action < allCards.Count ? allCards[actionRequest.Action] : null;

                if (targetNode is not null)
                {
                    foreach (var wrapper in playerActions)
                    {
                        if (wrapper.PlayerAction is PickCard pickCard && Equals(pickCard.CardNode, targetNode))
                        {
                            return wrapper;
                        }
                    }
                }
            }
            else
            {
                foreach (var wrapper in playerActions)
                {
                    if (wrapper.PlayerAction is TurnDecision turnDecision &&
                        turnDecision.PlayerAction is not null &&
                        turnDecision.PlayerAction.Id == actionRequest.Action)
                    {
                        return wrapper;
                    }
                }
            }

            return playerActions.First(w => w.CanPerform);
        }

        private ActionRequest? m_actionRequest;
        private readonly ManualResetEventSlim m_actionReady = new(false);
        private readonly IGameStateVectorReceiver m_gameStateVectorReceiver;
        private readonly IPlayerActionMaskReceiver m_playerActionMaskReceiver;
        private readonly IRewardCalculator m_rewardCalculator;
        private readonly IGame m_game;
    }
}
