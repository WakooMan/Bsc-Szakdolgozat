using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;

namespace SevenWonders.AI.Model.Services.CardTypeEncoders
{
    public class BlueCardEncoder : CardTypeEncoder<BlueCard>
    {
        protected override void EncodeCardType(BlueCard card, IDictionary<string, float> cardNodeProperties)
        {
            cardNodeProperties["CardType"] = 0.1f;
            cardNodeProperties[nameof(VictoryPoints)] = card.Point.Points / 100f;
        }
    }
}
