using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Elements.GameCards;

namespace SevenWonders.AI.Model.Services.CardTypeEncoders
{
    public class RedCardEncoder : CardTypeEncoder<RedCard>
    {
        protected override void EncodeCardType(RedCard card, IDictionary<string, float> cardNodeProperties)
        {
            cardNodeProperties["CardType"] = 0.2f;
            cardNodeProperties[nameof(Strength)] = card.Strength.Points / 100f;
        }
    }
}
