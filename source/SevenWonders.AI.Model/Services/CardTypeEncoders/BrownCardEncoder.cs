using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Goods.Resources;

namespace SevenWonders.AI.Model.Services.CardTypeEncoders
{
    public class BrownCardEncoder : CardTypeEncoder<BrownCard>
    {
        protected override void EncodeCardType(BrownCard card, IDictionary<string, float> cardNodeProperties)
        {
            cardNodeProperties["CardType"] = 0.5f;
            foreach (GameResource resource in card.ProducedResources)
            {
                cardNodeProperties[resource.GetType().Name] = resource.Amount / 10f;
            }
        }
    }
}
