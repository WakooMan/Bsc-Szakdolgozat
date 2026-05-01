using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Engine;
using System.Numerics;

namespace SevenWonders.Game.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class GrayCardChildTextureHandler : BaseCardChildTextureHandler<GrayCard>
    {
        public GrayCardChildTextureHandler(IGameEngineReceiver gameEngineReceiver, ITextureIdHandler textureIdHandler) : base("GrayCard", gameEngineReceiver, textureIdHandler)
        {
        }

        protected override void HandleCard(GrayCard card, GameObject gameObject)
        {
            Sprite? frontSprite = gameObject.Animations.FirstOrDefault(s => s.Name == "front");
            if (frontSprite is null || frontSprite.Frames.Count == 0)
            {
                return;
            }

            float iconWidthPercent = 0.15f;
            float iconHeightPercent = 0.15f;
            List<ChildObject> childObjects = new List<ChildObject>();

            card.CreatedProducts.ForEach(product =>
            {
                int productTextureId = m_textureIdHandler.GetTextureId(product.GetType().Name);
                for (int i = 0; i < product.Amount; i++)
                {
                    childObjects.Add(new ChildTexture
                    {
                        TextureId = productTextureId,
                        WidthPercent = iconWidthPercent,
                        HeightPercent = iconHeightPercent,
                    });
                }
            });

            float groupStartX = (1f - childObjects.Count * iconWidthPercent) / 2f;
            float centeredY = (0.2f - iconHeightPercent);

            for (int i = 0; i < childObjects.Count; i++)
            {
                float posX = groupStartX + i * iconWidthPercent;
                childObjects[i].PositionPercent = new Vector2(posX, centeredY);
                frontSprite.AddChildObject(childObjects[i]);
            }
        }
    }
}
