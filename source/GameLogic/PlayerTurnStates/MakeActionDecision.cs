using GameLogic.Elements;
using GameLogic.Events.GameEvents;
using GameLogic.PlayerActions;
using SevenWonders.Common;

namespace GameLogic.PlayerTurnStates
{
    public class MakeActionDecision : IPlayerTurnState
    {
        public bool GoToPrevState { get; set; }

        public MakeActionDecision(IGameContext gameContext)
        {
            m_gameContext = gameContext;
            GoToPrevState = false;
        }

        public void ExecuteTurnState()
        {
            GameLog.Info($"Player={CurrentPlayer.Name} making action decision.");
            Action<OnCardUnpicked> action = (args) => GoToPrevState = true;
            m_gameContext.EventManager.Subscribe(action);
            List<IPlayerAction> playerActions =
            [
                new TurnDecision(new UnpickCard(CurrentPlayer)), new TurnDecision(new BuildCard()), new TurnDecision(new SellCard(CurrentPlayer)),
                new TurnDecision(new BuildWonderProcess(CurrentPlayer, CurrentPlayer.Wonders.Select(wonder => new BuildWonder(wonder)).ToList())),
            ];
            m_gameContext.PlayerActionHandler.HandlePlayerActionsCompleted(m_gameContext, CurrentPlayer, playerActions);
            m_gameContext.EventManager.Unsubscribe(action);
            GameLog.Info($"Player={CurrentPlayer.Name} decision completed. GoToPrevState={GoToPrevState}");
        }

        public IPlayerTurnState GetNextTurnState()
        {
            return GoToPrevState ? new PickCardState(m_gameContext) : new EndTurn();
        }

        private Player CurrentPlayer => m_gameContext.TurnHandler.CurrentPlayer;

        private readonly IGameContext m_gameContext;
    }
}
