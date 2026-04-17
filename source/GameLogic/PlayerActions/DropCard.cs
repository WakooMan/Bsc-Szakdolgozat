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
        public DropCard(Player owner, Player opponent, Card card)
        {
            ArgumentChecker.CheckNull(owner, nameof(owner));
            ArgumentChecker.CheckNull(opponent, nameof(opponent));
            ArgumentChecker.CheckNull(card, nameof(card));

            m_owner = owner;
            m_opponent = opponent;
            m_card = card;
        }
        public Task<bool> CanPerform(IGameContext gameContext)
        {
            return Task.FromResult(m_owner.Cards.Contains(m_card));
        }

        public async Task<bool> DoPlayerAction(IGameContext gameContext)
        {
            ArgumentChecker.CheckPredicateForOperation(() => !m_owner.Cards.Contains(m_card), "Player does not have the specific card! Action cannot be performed!");

            m_owner.Cards.Remove(m_card);
            gameContext.DroppedCardList.Cards.Add(m_card);
            await m_card.OnDestroyed(gameContext, m_owner, m_opponent);
            await gameContext.EventManager.PublishAsync(new OnCardDestroyed(m_owner, m_card));
            return true;
        }

        private readonly Card m_card;
        private readonly Player m_owner;
        private readonly Player m_opponent;
    }
}
