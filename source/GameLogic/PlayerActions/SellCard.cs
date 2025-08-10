using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;
using SevenWonders.Common;

namespace GameLogic.PlayerActions
{
    public class SellCard : IPlayerAction
    {
        public SellCard(Player player)
        {
            ArgumentChecker.CheckNull(player, nameof(player));

            m_player = player;
        }

        public void DoPlayerAction(IGameContext gameContext)
        {
            if (m_player.PickedCard is null)
            {
                throw new InvalidOperationException("Cannot execute action if player did not pick a card to sell.");
            }

            gameContext.AgeHandler.CurrentAge.Composition.RemoveCard(m_player.PickedCard);
            int money = 2 + m_player.Cards.OfType<YellowCard>().Count();
            m_player.Money += money;
            Card card = m_player.PickedCard.CardObj;
            m_player.PickedCard = null;
            gameContext.EventManager.Publish(new OnCardSold(m_player, card, money));
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return m_player.PickedCard is not null;
        }

        private readonly Player m_player;
    }
}
