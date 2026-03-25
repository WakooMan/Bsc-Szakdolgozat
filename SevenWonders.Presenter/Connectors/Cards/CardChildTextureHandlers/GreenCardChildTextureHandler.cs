using GameLogic.Elements.GameCards;
using SevenWonders.GameEngine;

namespace SevenWonders.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class GreenCardChildTextureHandler : BaseCardChildTextureHandler<GreenCard>
    {
        public GreenCardChildTextureHandler(IGameEngineReceiver gameEngineReceiver) : base(TextureIdDictionary.GetTextureId("GreenCardHeader"), gameEngineReceiver)
        {
        }

        protected override void HandleCard(GreenCard card, GameObject gameObject)
        {
            // Implement texture handling logic specific to GreenCard here
        }
    }
}