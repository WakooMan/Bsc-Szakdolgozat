using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;

namespace GameLogic.PlayerActions
{
    public class ChooseCardAction : IPlayerAction
    {
        public ChooseCardAction(IGameContext gameContext, Card card)
        {
            m_card = card;
            m_gameContext = gameContext;
        }

        public bool CanPerform()
        {
            return true;
        }

        public void DoPlayerAction()
        {
            Player player = m_gameContext.TurnHandler.CurrentPlayer;
            m_gameContext.EventManager.Publish(GameEventType.CardBuilt, new OnCardBuilt(m_card, player, 0, false));
            m_card.OnBuilt(m_gameContext);
        }

        private readonly Card m_card;
        private readonly IGameContext m_gameContext;

    }
}
