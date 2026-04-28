using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using SevenWonders.AI.Model.Services.Encoders;

namespace SevenWonders.AI.Model.Services.CardTypeEncoders
{
    public class YellowCardEncoder : CardTypeEncoder<YellowCard>
    {
        public YellowCardEncoder(IEffectEncoder effectEncoder)
        {
            m_effectEncoder = effectEncoder;
        }

        protected override void EncodeCardType(YellowCard card, IDictionary<string, float> cardNodeProperties)
        {
            cardNodeProperties["CardType"] = 0.7f;
            foreach (Effect effect in card.Effects)
            {
                m_effectEncoder.EncodeEffect(effect, cardNodeProperties);
            }
        }

        private readonly IEffectEncoder m_effectEncoder;
    }
}
