using GameLogic;
using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.GameStructures;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using SevenWonders.Common;

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
            GameLog.Info($"Player={CurrentPlayer.Name} picking from {Composition.AvailableCards.Count} available cards.");
            m_gameContext.PlayerActionHandler.HandlePlayerActions(m_gameContext, CurrentPlayer, Composition.AvailableCards.Select(card => (IPlayerAction)new PickCard(CurrentPlayer, card)).ToList());
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
