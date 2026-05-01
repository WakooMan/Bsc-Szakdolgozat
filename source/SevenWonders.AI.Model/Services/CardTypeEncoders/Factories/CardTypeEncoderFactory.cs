using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.AI.Model.Services.Encoders;

namespace SevenWonders.AI.Model.Services.CardTypeEncoders.Factories
{
    public class CardTypeEncoderFactory : ICardTypeEncoderFactory
    {
        public CardTypeEncoderFactory(IEffectEncoder effectEncoder)
        {
            m_effectEncoder = effectEncoder;
            m_typePairs = new Dictionary<Type, Func<ICardTypeEncoder>>()
            {
                { typeof(BlueCard), () => new BlueCardEncoder() },
                { typeof(GreenCard), () => new GreenCardEncoder() },
                { typeof(GrayCard), () => new GrayCardEncoder() },
                { typeof(RedCard), () => new RedCardEncoder() },
                { typeof(YellowCard), () => new YellowCardEncoder(m_effectEncoder) },
                { typeof(BrownCard), () => new BrownCardEncoder() },
                { typeof(PurpleCard), () => new PurpleCardEncoder() }
            };
        }

        public ICardTypeEncoder? Create(Type cardType)
        {
            if (m_typePairs.ContainsKey(cardType))
            {
                return m_typePairs[cardType]();
            }

            return null;
        }

        private readonly IDictionary<Type, Func<ICardTypeEncoder>> m_typePairs;
        private readonly IEffectEncoder m_effectEncoder;
    }
}
