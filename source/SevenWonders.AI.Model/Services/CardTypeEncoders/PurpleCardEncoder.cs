using GameLogic.Elements.GameCards;

namespace SevenWonders.AI.Model.Services.CardTypeEncoders
{
    public class PurpleCardEncoder : CardTypeEncoder<PurpleCard>
    {
        protected override void EncodeCardType(PurpleCard card, IDictionary<string, float> cardNodeProperties)
        {
            cardNodeProperties["CardType"] = 0.6f;
            cardNodeProperties[card.GuildObj.GetType().Name] = 1f;
        }
    }
}
