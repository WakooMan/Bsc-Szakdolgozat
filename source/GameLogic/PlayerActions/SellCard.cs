using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;

namespace GameLogic.PlayerActions
{
    public class SellCard : IPlayerAction
    {
        public SellCard(IEventManager eventManager, ICardComposition composition, Player player)
        {
            m_eventManager = eventManager;
            m_composition = composition;
            m_player = player;
        }

        public void DoPlayerAction()
        {
            if (m_player.PickedCard is null)
            {
                throw new InvalidOperationException("Cannot execute action if player did not pick a card to sell.");
            }
            m_composition.RemoveCard(m_player.PickedCard);
            int money = 2 + m_player.Cards.Where(card => card is YellowCard).Count();
            m_player.Money += money;
            Card card = m_player.PickedCard.CardObj;
            m_player.PickedCard = null;
            m_eventManager.Publish(new OnCardSold(m_player, card, money));
        }

        public bool CanPerform()
        {
            return true;
        }

        private readonly IEventManager m_eventManager;
        private readonly ICardComposition m_composition;
        private readonly Player m_player;
    }
}
