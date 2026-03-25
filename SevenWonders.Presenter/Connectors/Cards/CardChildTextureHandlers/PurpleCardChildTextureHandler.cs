using GameLogic.Elements.GameCards;
using SevenWonders.GameEngine;

namespace SevenWonders.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class PurpleCardChildTextureHandler: BaseCardChildTextureHandler<PurpleCard>
    {
        public PurpleCardChildTextureHandler(IGameEngineReceiver gameEngineReceiver) : base(TextureIdDictionary.GetTextureId("PurpleCardHeader"), gameEngineReceiver)
        {
        }

        protected override void HandleCard(PurpleCard card, GameObject gameObject)
        {
            // Implement texture handling logic specific to PurpleCard here
        }
    }
}
