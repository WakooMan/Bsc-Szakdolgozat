using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using SevenWonders.Common;

namespace GameLogic.PlayerActions
{
    public class DropCard : IPlayerAction
    {
        public string Name => m_card.Name;
        public DropCard() { }
        public DropCard(Player player, Card card)
        {
            ArgumentChecker.CheckNull(player, nameof(player));
            ArgumentChecker.CheckNull(card, nameof(card));

            m_player = player;
            m_card = card;
        }
        public Task<bool> CanPerform(IGameContext gameContext)
        {
            return Task.FromResult(m_player.Cards.Contains(m_card));
        }

        public async Task<bool> DoPlayerAction(IGameContext gameContext)
        {
            ArgumentChecker.CheckPredicateForOperation(() => !m_player.Cards.Contains(m_card), "Player does not have the specific card! Action cannot be performed!");

            m_player.Cards.Remove(m_card);
            await gameContext.EventManager.PublishAsync(new OnCardDestroyed(m_player, m_card));
            return true;
        }

        private readonly Card m_card;
        private readonly Player m_player;
    }
}
