using GameLogic.Elements.GameCards;
using SevenWonders.GameEngine;

namespace SevenWonders.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class BrownCardChildTextureHandler: BaseCardChildTextureHandler<BrownCard>
    {
        public BrownCardChildTextureHandler(IGameEngineReceiver gameEngineReceiver) : base(TextureIdDictionary.GetTextureId("BrownCardHeader"), gameEngineReceiver)
        {
        }

        protected override void HandleCard(BrownCard card, GameObject gameObject)
        {
            // Implement texture handling logic specific to BrownCard here
        }
    }
}
