using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Common;

namespace SevenWonders.Game.Logic.PlayerActions
{
    public class SellCard : IPlayerAction
    {
        public string Name => nameof(SellCard);
        public int Id => 21;
        public SellCard(Player player)
        {
            ArgumentChecker.CheckNull(player, nameof(player));

            m_player = player;
        }

        public bool DoPlayerAction(IGameContext gameContext)
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
            gameContext.DroppedCardList.Cards.Add(card);
            gameContext.EventManager.Publish(new OnCardSold(m_player, card, money));
            return true;
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return m_player.PickedCard is not null;
        }

        private readonly Player m_player;
    }
}
