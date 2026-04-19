using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods.Products;

namespace SevenWonders.AI.Model.Services.CardTypeEncoders
{
    public class GrayCardEncoder : CardTypeEncoder<GrayCard>
    {
        protected override void EncodeCardType(GrayCard card, IDictionary<string, float> cardNodeProperties)
        {
            cardNodeProperties["CardType"] = 0.4f;
            foreach (Product product in card.CreatedProducts)
            {
                cardNodeProperties[product.GetType().Name] = product.Amount / 10f;
            }
        }
    }
}
