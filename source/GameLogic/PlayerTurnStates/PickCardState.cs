using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.GameStructures;
using GameLogic.PlayerActions;

namespace GameLogic.PlayerTurnStates
{
    public class PickCardState : IPlayerTurnState
    {

        public PickCardState(IGameContext gameContext)
        {
            m_gameContext = gameContext;
        }

        public void ExecuteTurnState()
        {
            IPlayerAction playerAction = m_gameContext.PlayerActionReceiver.ReceivePlayerAction(CurrentPlayer, Composition.AvailableCards.Select(card => (IPlayerAction)new PickCard(CurrentPlayer, card)).ToList());
            if (playerAction.CanPerform(m_gameContext))
            {
                playerAction.DoPlayerAction(m_gameContext);
            }
        }

        public IPlayerTurnState GetNextTurnState()
        {
            return new MakeActionDecision(m_gameContext);
        }

        private ICardComposition Composition => m_gameContext.AgeHandler.CurrentAge.Composition;
        private Player CurrentPlayer => m_gameContext.TurnHandler.CurrentPlayer;

        private readonly IGameContext m_gameContext;
    }
}
