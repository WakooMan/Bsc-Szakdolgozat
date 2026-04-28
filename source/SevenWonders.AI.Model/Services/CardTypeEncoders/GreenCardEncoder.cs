using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;

namespace SevenWonders.AI.Model.Services.CardTypeEncoders
{
    public class GreenCardEncoder: CardTypeEncoder<GreenCard>
    {
        protected override void EncodeCardType(GreenCard card, IDictionary<string, float> cardNodeProperties)
        {
            cardNodeProperties["CardType"] = 0.3f;
            cardNodeProperties[card.Discipline.GetType().Name] = 1f;
            cardNodeProperties[nameof(VictoryPoints)] = card.Point.Points / 100f;
        }
    }
}
