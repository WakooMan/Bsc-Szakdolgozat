using GameLogic.Elements.GameCards;
using SevenWonders.GameEngine;

namespace SevenWonders.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class GrayCardChildTextureHandler : BaseCardChildTextureHandler<GrayCard>
    {
        public GrayCardChildTextureHandler(IGameEngineReceiver gameEngineReceiver) : base(TextureIdDictionary.GetTextureId("GrayCardHeader"), gameEngineReceiver)
        {
        }

        protected override void HandleCard(GrayCard card, GameObject gameObject)
        {
            // Implement texture handling logic specific to GrayCard here
        }
    }
}
