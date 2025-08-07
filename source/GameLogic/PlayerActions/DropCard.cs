using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;

namespace GameLogic.PlayerActions
{
    public class DropCard : IPlayerAction
    {
        public DropCard(Player player, Card card)
        {
            m_player = player;
            m_card = card;
        }
        public bool CanPerform(IGameContext gameContext)
        {
            return m_player.Cards.Contains(m_card);
        }

        public void DoPlayerAction(IGameContext gameContext)
        {
            m_player.Cards.Remove(m_card);
            gameContext.EventManager.Publish(new OnCardDestroyed(m_player, m_card));
        }

        private readonly Card m_card;
        private readonly Player m_player;
    }
}
