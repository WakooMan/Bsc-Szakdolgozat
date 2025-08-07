using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;

namespace GameLogic.PlayerActions
{
    public class UnpickCard : IPlayerAction
    {
        public UnpickCard(Player player)
        {
            m_player = player;
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return m_player.PickedCard is not null;
        }

        public void DoPlayerAction(IGameContext gameContext)
        {
            if (m_player.PickedCard is null)
            {
                throw new InvalidOperationException("Cannot perform action if picked card is null!");
            }

            Card card = m_player.PickedCard.CardObj;
            m_player.PickedCard = null;
            gameContext.EventManager.Publish(new OnCardUnpicked(m_player, card));
        }

        private readonly Player m_player;
    }
}
