using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.GameStructures;
using GameLogic.Handlers;

namespace GameLogic.PlayerActions
{
    public class BuildCard : IPlayerAction
    {
        public BuildCard(IEventManager eventManager, ICostCalculator costCalculator, ICardComposition composition, Player player, Player opponent)
        {
            m_eventManager = eventManager;
            m_costCalculator = costCalculator;
            m_composition = composition;
            m_player = player;
            m_opponent = opponent;
        }

        public void DoPlayerAction()
        {
            if (m_player.PickedCard is null)
            {
                throw new InvalidOperationException($"{m_player.Name} player's picked card is null, {nameof(BuildCard)} action cannot be performed!");
            }

            ICardNode card = m_player.PickedCard;
            m_composition.RemoveCard(card);
            m_player.Cards.Add(card.CardObj);
            if (string.IsNullOrEmpty(card.CardObj.PreviousBuilding) ||
               m_player.Cards.All(c => c.Name != card.CardObj.PreviousBuilding))
            {
                m_player.Money -= m_costCalculator.GetBuildCost(card.CardObj, m_player, m_opponent);
            }

            m_eventManager.Publish(GameEventType.CardBuilt, new OnCardBuilt(card.CardObj, m_player));
            card.CardObj.OnBuilt(m_player, m_eventManager);

        }

        public bool CanPerform()
        {
            if (m_player.PickedCard is null)
            {
                return false;
            }

            Card card = m_player.PickedCard.CardObj;

            if (!string.IsNullOrEmpty(card.PreviousBuilding) &&
               m_player.Cards.Any(c => c.Name == card.PreviousBuilding))
                return true;


            return m_costCalculator.CanAfford(card, m_player, m_opponent);
        }

        private readonly ICardComposition m_composition;
        private readonly Player m_player;
        private readonly Player m_opponent;
        private readonly ICostCalculator m_costCalculator;
        private readonly IEventManager m_eventManager;
    }
}
