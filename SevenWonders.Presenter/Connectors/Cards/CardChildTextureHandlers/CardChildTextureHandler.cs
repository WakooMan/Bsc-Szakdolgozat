using GameLogic.Elements.GameCards;
using SevenWonders.Presenter.Connectors.Effects;

namespace SevenWonders.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class CardChildTextureHandler: ICardChildTextureHandler
    {
        public CardChildTextureHandler(IGameEngineReceiver gameEngineReceiver, IEffectHandler effectHandler)
        {
            m_gameEngineReceiver = gameEngineReceiver;
            m_cardTypeTextureHandlers = new()
            {
                { typeof(BlueCard),   new BlueCardChildTextureHandler(m_gameEngineReceiver) },
                { typeof(BrownCard),  new BrownCardChildTextureHandler(m_gameEngineReceiver) },
                { typeof(GrayCard),   new GrayCardChildTextureHandler(m_gameEngineReceiver) },
                { typeof(GreenCard),  new GreenCardChildTextureHandler(m_gameEngineReceiver) },
                { typeof(PurpleCard), new PurpleCardChildTextureHandler(m_gameEngineReceiver) },
                { typeof(RedCard),    new RedCardChildTextureHandler(m_gameEngineReceiver) },
                { typeof(YellowCard), new YellowCardChildTextureHandler(m_gameEngineReceiver, effectHandler) },
            };
        }

        public void Handle(Card card)
        {
            if (m_cardTypeTextureHandlers.TryGetValue(card.GetType(), out ICardChildTextureHandler? cardChildTextureHandler))
            {
                cardChildTextureHandler.Handle(card);
            }
        }

        private readonly Dictionary<Type, ICardChildTextureHandler> m_cardTypeTextureHandlers;

        private readonly IGameEngineReceiver m_gameEngineReceiver;
    }
}
