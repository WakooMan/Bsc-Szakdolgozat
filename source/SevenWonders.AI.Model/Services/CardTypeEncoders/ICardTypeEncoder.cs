using GameLogic.Elements.GameCards;

namespace SevenWonders.AI.Model.Services.CardTypeEncoders
{
    public interface ICardTypeEncoder
    {
        void EncodeCard(Card card, IDictionary<string, float> cardNodeProperties);
    }
}
