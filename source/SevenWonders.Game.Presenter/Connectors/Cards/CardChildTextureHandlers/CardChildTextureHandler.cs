using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Presenter.Connectors.Effects;

namespace SevenWonders.Game.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class CardChildTextureHandler: ICardChildTextureHandler
    {
        public CardChildTextureHandler(IGameEngineReceiver gameEngineReceiver, IEffectHandler effectHandler, ITextureIdHandler textureIdHandler)
        {
            m_gameEngineReceiver = gameEngineReceiver;
            m_cardTypeTextureHandlers = new()
            {
                { typeof(BlueCard),   new BlueCardChildTextureHandler(m_gameEngineReceiver, textureIdHandler) },
                { typeof(BrownCard),  new BrownCardChildTextureHandler(m_gameEngineReceiver, textureIdHandler) },
                { typeof(GrayCard),   new GrayCardChildTextureHandler(m_gameEngineReceiver, textureIdHandler) },
                { typeof(GreenCard),  new GreenCardChildTextureHandler(m_gameEngineReceiver, textureIdHandler) },
                { typeof(PurpleCard), new PurpleCardChildTextureHandler(m_gameEngineReceiver, textureIdHandler) },
                { typeof(RedCard),    new RedCardChildTextureHandler(m_gameEngineReceiver, textureIdHandler) },
                { typeof(YellowCard), new YellowCardChildTextureHandler(m_gameEngineReceiver, effectHandler, textureIdHandler) },
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
