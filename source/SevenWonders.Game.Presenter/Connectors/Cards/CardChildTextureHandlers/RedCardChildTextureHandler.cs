using SevenWonders.Game.Logic.Elements.GameCards;
using System.Numerics;
using SevenWonders.Game.Engine.SceneObjects;
using SevenWonders.Game.Engine.ChildObjects;

namespace SevenWonders.Game.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class RedCardChildTextureHandler: BaseCardChildTextureHandler<RedCard>
    {
        public RedCardChildTextureHandler(IGameEngineReceiver gameEngineReceiver, ITextureIdHandler textureIdHandler) : base("RedCard", gameEngineReceiver, textureIdHandler)
        {
        }

        protected override void HandleCard(RedCard card, GameObject gameObject)
        {
            Sprite? frontSprite = gameObject.Animations.FirstOrDefault(s => s.Name == "front");
            if (frontSprite is null || frontSprite.Frames.Count == 0)
            {
                return;
            }

            int strengthCount = card.Strength.Points;
            if (strengthCount == 0)
            {
                return;
            }

            int strengthTextureId = m_textureIdHandler.GetTextureId("Military");

            float iconWidthPercent = 0.15f;
            float iconHeightPercent = 0.15f;

            float groupStartX = (1f - strengthCount * iconWidthPercent) / 2f;
            float centeredY = (0.2f - iconHeightPercent);

            for (int i = 0; i < strengthCount; i++)
            {
                float posX = groupStartX + i * iconWidthPercent;
                ChildTexture childTexture = new ChildTexture
                {
                    TextureId = strengthTextureId,
                    WidthPercent = iconWidthPercent,
                    HeightPercent = iconHeightPercent,
                    PositionPercent = new Vector2(posX, centeredY)
                };
                frontSprite.AddChildObject(childTexture);
            }
        }
    }
}
