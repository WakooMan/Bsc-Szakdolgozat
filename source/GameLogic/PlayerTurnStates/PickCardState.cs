using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.GameStructures;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;

namespace GameLogic.PlayerTurnStates
{
    public class PickCardState : IPlayerTurnState
    {

        public PickCardState(IGameContext gameContext)
        {
            m_gameContext = gameContext;
        }

        public async Task ExecuteTurnState()
        {
            await m_gameContext.PlayerActionHandler.HandlePlayerActions(m_gameContext, CurrentPlayer, Composition.AvailableCards.Select(card => (IPlayerAction)new PickCard(CurrentPlayer, card)).ToList());
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
