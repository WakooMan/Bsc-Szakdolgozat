using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using SevenWonders.Common;

namespace GameLogic.PlayerActions
{
    public class DropCard : IPlayerAction
    {
        public DropCard(Player player, Card card)
        {
            ArgumentChecker.CheckNull(player, nameof(player));
            ArgumentChecker.CheckNull(card, nameof(card));

            m_player = player;
            m_card = card;
        }
        public bool CanPerform(IGameContext gameContext)
        {
            return m_player.Cards.Contains(m_card);
        }

        public void DoPlayerAction(IGameContext gameContext)
        {
            ArgumentChecker.CheckPredicateForOperation(() => !m_player.Cards.Contains(m_card), "Player does not have the specific card! Action cannot be performed!");

            m_player.Cards.Remove(m_card);
            gameContext.EventManager.Publish(new OnCardDestroyed(m_player, m_card));
        }

        private readonly Card m_card;
        private readonly Player m_player;
    }
}
