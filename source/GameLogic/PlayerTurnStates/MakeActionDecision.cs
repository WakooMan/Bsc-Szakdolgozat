using GameLogic.Elements;
using GameLogic.Events.GameEvents;
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
            Action<OnCardUnpicked> action = (args) => GoToPrevState = true;
            m_gameContext.EventManager.Subscribe(action);
            List<TurnDecision> playerActions =
            [
                new TurnDecision(new UnpickCard(CurrentPlayer)), new TurnDecision(new BuildCard()), new TurnDecision(new SellCard(CurrentPlayer)),
                .. CurrentPlayer.Wonders.Select(wonder => new TurnDecision(new BuildWonder(wonder))),
            ];

            var playerAction = m_gameContext.PlayerActionReceiver.ReceivePlayerAction(CurrentPlayer, playerActions);
            if (playerAction.CanPerform(m_gameContext))
            {
                playerAction.DoPlayerAction(m_gameContext);
            }

            m_gameContext.EventManager.Unsubscribe(action);
        }

        public IPlayerTurnState GetNextTurnState()
        {
            return GoToPrevState ? new PickCardState(m_gameContext) : new EndTurn();
        }

        private Player CurrentPlayer => m_gameContext.TurnHandler.CurrentPlayer;

        private readonly IGameContext m_gameContext;
    }
}
