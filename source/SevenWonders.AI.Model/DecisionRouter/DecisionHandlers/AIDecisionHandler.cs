using GameLogic;
using GameLogic.Elements;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using SevenWonders.AI.Model.Messages;
using SevenWonders.AI.Model.Services;
using SevenWonders.Common;
using System.Numerics;

namespace SevenWonders.AI.Model.DecisionRouter.DecisionHandlers
{
    public class AIDecisionHandler : IAIDecisionHandler
    {
        public Func<GameStateResponse, ActionRequest>? OnGameStateReceived { get; set; }

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
        }

        public void Initialize(int playerId)
        {
            GameLog.Info("Initializing...");
            m_playerId = playerId;
            m_gameStateVectorReceiver.Initialize();
            m_playerActionMaskReceiver.Initialize();
            m_rewardCalculator.Reset();
            m_game.Context.EventManager.Subscribe<OnGameEnded>(GameEnded);
            m_game.Context.EventManager.Subscribe<MilitaryVictory>(OnMilitaryVictory);
            m_game.Context.EventManager.Subscribe<ScientificVictory>(OnScientificVictory);
            GameLog.Info("Initialized and subscribed to events.");
        }

        public void Uninitialize()
        {
            GameLog.Info("Uninitializing and unsubscribing from events...");
            m_playerId = -1;
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
                Reward = m_rewardCalculator.CalculateInstantWinReward(playerProperties, m_playerId),
                Terminated = true
            };
            OnGameStateReceived?.Invoke(messageObj);
        }

        private void OnScientificVictory(ScientificVictory victory)
        {
            GameLog.Info($"Scientific victory detected! Player={victory.PlayerProperties.Owner.Name}");
            PlayerProperties loserProp = victory.PlayerProperties.Opponent.GetPlayerProperties(victory.PlayerProperties.Owner);
            PlayerProperties playerProperties = victory.PlayerProperties.Owner.Id == m_playerId ? victory.PlayerProperties : loserProp;
            PlayerProperties opponentProperties = victory.PlayerProperties.Owner.Id == m_playerId ? loserProp : victory.PlayerProperties;
            PhaseIndicator phase = PhaseIndicator.ChooseAction;
            List<float> stateVector = m_gameStateVectorReceiver.Receive(playerProperties, opponentProperties, phase);
            List<int> actionMask = m_playerActionMaskReceiver.ReceiveEmptyPlayerActionMask();
            GameStateResponse messageObj = new GameStateResponse
            {
                State = stateVector,
                Mask = actionMask,
                Reward = m_rewardCalculator.CalculateInstantWinReward(playerProperties, m_playerId),
                Terminated = true
            };
            OnGameStateReceived?.Invoke(messageObj);
        }

        private void GameEnded(OnGameEnded ended)
        {
            GameLog.Info($"Game ended! Player1={ended.FirstPlayer.Owner.Name} VP={ended.FirstPlayer.VictoryPoints}, Player2={ended.SecondPlayer.Owner.Name} VP={ended.SecondPlayer.VictoryPoints}");
            PlayerProperties playerProperties = (ended.FirstPlayer.Owner.Id == m_playerId) ? ended.FirstPlayer : ended.SecondPlayer;
            PlayerProperties opponentProperties = (ended.FirstPlayer.Owner.Id == m_playerId) ? ended.SecondPlayer : ended.FirstPlayer;
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

        public PlayerActionWrapper HandleDecisions(Player player, ICollection<PlayerActionWrapper> playerActions)
        {
            GameLog.Info($"HandleDecisions: Player={player.Name}, ActionCount={playerActions.Count}");
            PlayerActionWrapper[] actionsArray = playerActions.ToArray();
            PhaseIndicator phase = DeterminePhase(actionsArray);

            ActionRequest? actionRequest = null;
            GameStateResponse messageObj = CreateStateResponse(player, phase, actionsArray, validAction: true);
            while (actionRequest is null)
            {
                GameLog.Info($"Phase={phase}");
                string mask = "[ ";
                messageObj.Mask.ForEach(m => mask += $"{m}, ");
                mask += "]";
                GameLog.Info($"mask: {mask}");
                GameLog.Info($"Sending game state. Reward={messageObj.Reward}, Terminated={messageObj.Terminated}");
                actionRequest = OnGameStateReceived?.Invoke(messageObj) ?? null;
                if (actionRequest is not null)
                {
                    PlayerActionWrapper? action = null;
                    action = MapActionToWrapper(actionRequest, phase, actionsArray);
                    if (action is not null)
                    {
                        GameLog.Info($"Mapped to action: {action.PlayerAction.GetType().Name}, CanPerform={action.CanPerform}");
                        return action;
                    }
                    else
                    {
                        GameLog.Warning($"Received invalid action request: {actionRequest.Action}. Waiting for a valid action...");
                        actionRequest = null;
                        messageObj = CreateStateResponse(player, phase, actionsArray, validAction: false);
                    }
                }
                else
                {
                    GameLog.Warning("Received null action request. Waiting for a valid action...");
                    messageObj = CreateStateResponse(player, phase, actionsArray, validAction: false);
                }
            }

            throw new InvalidOperationException("Failed to receive a valid action request.");
        }

        private static PhaseIndicator DeterminePhase(PlayerActionWrapper[] playerActions)
        {
            if (playerActions.Length > 0 && playerActions[0].PlayerAction is PickCard)
            {
                return PhaseIndicator.ChooseCard;
            }
            return PhaseIndicator.ChooseAction;
        }

        private GameStateResponse CreateStateResponse(Player player, PhaseIndicator phase, PlayerActionWrapper[] actionsArray, bool validAction)
        {
            Player opponent = m_game.Players.First(p => p.Id != player.Id);
            PlayerProperties playerProperties = player.GetPlayerProperties(opponent);
            PlayerProperties opponentProperties = opponent.GetPlayerProperties(player);

            List<float> stateVector = m_gameStateVectorReceiver.Receive(playerProperties, opponentProperties, phase);
            List<int> actionMask = m_playerActionMaskReceiver.ReceivePlayerActionMask(phase, actionsArray);

            GameStateResponse messageObj = new GameStateResponse
            {
                State = stateVector,
                Mask = actionMask,
                Reward = validAction ? m_rewardCalculator.CalculateTurnReward(playerProperties, opponentProperties) : -1f,
                Terminated = false
            };
            return messageObj;
        }

        private PlayerActionWrapper? MapActionToWrapper(ActionRequest actionRequest, PhaseIndicator phase, PlayerActionWrapper[] playerActions)
        {
            if (phase == PhaseIndicator.ChooseCard)
            {
                ICardNode? targetNode = m_playerActionMaskReceiver.GetNode(actionRequest.Action);

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
                        turnDecision.PlayerAction is not UnpickCard &&
                        turnDecision.PlayerAction.Id == actionRequest.Action)
                    {
                        return wrapper;
                    }
                }
            }

            return null;
        }

        private int m_playerId = -1;
        private readonly IGameStateVectorReceiver m_gameStateVectorReceiver;
        private readonly IPlayerActionMaskReceiver m_playerActionMaskReceiver;
        private readonly IRewardCalculator m_rewardCalculator;
        private readonly IGame m_game;
    }
}
