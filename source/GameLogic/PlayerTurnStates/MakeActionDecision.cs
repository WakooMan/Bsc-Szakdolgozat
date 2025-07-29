using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.GameStructures;
using GameLogic.PlayerActions;

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
            Action<EventArgs> action = (args) => GoToPrevState = true;
            m_gameContext.EventManager.Subscribe(GameEventType.CardUnpicked, action);
            List<IPlayerAction> playerActions =
            [
                new UnpickCard(m_gameContext.EventManager, CurrentPlayer), new BuildCard(m_gameContext), new SellCard(Composition, CurrentPlayer),
                .. CurrentPlayer.Wonders.Select(wonder => new BuildWonder(m_gameContext, wonder)),
            ];

            m_gameContext.PlayerActionReceiver.ReceivePlayerAction(CurrentPlayer, playerActions).DoPlayerAction();
            m_gameContext.EventManager.Unsubscribe(GameEventType.CardUnpicked, action);
        }

        public IPlayerTurnState GetNextTurnState()
        {
            return GoToPrevState ? new PickCardState(m_gameContext) : new EndTurn();
        }

        private Player CurrentPlayer => m_gameContext.TurnHandler.CurrentPlayer;
        private ICardComposition Composition => m_gameContext.AgeHandler.CurrentAge.Composition;

        private readonly IGameContext m_gameContext;
    }
}
