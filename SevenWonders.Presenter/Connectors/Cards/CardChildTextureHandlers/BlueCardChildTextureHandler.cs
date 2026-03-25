using GameLogic.Elements.GameCards;
using SevenWonders.GameEngine;

namespace SevenWonders.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class BlueCardChildTextureHandler : BaseCardChildTextureHandler<BlueCard>
    {
        public BlueCardChildTextureHandler(IGameEngineReceiver gameEngineReceiver) : base(TextureIdDictionary.GetTextureId("BlueCardHeader"), gameEngineReceiver)
        {
        }

        protected override void HandleCard(BlueCard card, GameObject gameObject)
        {
            // Implement texture handling logic specific to BlueCard here
        }
    }
}
