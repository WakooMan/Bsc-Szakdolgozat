using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.GameStructures;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Game.Logic.PlayerActions;
using SevenWonders.Common;

namespace SevenWonders.Game.Logic.PlayerTurnStates
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
