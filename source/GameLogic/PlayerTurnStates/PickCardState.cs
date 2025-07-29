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
            m_gameContext.PlayerActionReceiver.ReceivePlayerAction(CurrentPlayer, Composition.AvailableCards.Select(card => (IPlayerAction)new PickCard(m_gameContext.EventManager, CurrentPlayer, card, Composition)).ToList()).DoPlayerAction();
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
