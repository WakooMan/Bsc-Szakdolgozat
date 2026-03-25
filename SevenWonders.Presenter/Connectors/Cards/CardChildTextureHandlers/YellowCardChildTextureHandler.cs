using GameLogic.Elements.GameCards;
using SevenWonders.GameEngine;

namespace SevenWonders.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class YellowCardChildTextureHandler: BaseCardChildTextureHandler<YellowCard>
    {
        public YellowCardChildTextureHandler(IGameEngineReceiver gameEngineReceiver) : base(TextureIdDictionary.GetTextureId("YellowCardHeader"), gameEngineReceiver)
        {
        }

        protected override void HandleCard(YellowCard card, GameObject gameObject)
        {
            // Implement texture handling logic specific to YellowCard here
        }
    }
}
