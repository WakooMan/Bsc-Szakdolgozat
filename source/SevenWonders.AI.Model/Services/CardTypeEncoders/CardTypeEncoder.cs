using GameLogic.Elements.GameCards;

namespace SevenWonders.AI.Model.Services.CardTypeEncoders
{
    public abstract class CardTypeEncoder<TCard> : ICardTypeEncoder where TCard : Card
    {
        public void EncodeCard(Card card, IDictionary<string, float> cardNodeProperties)
        {
            if (card is TCard typedCard)
            {
                EncodeCardType(typedCard, cardNodeProperties);
            }
        }

        protected abstract void EncodeCardType(TCard card, IDictionary<string, float> cardNodeProperties);
    }
}
