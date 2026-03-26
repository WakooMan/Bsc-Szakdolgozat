using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using SevenWonders.GameEngine;
using SkiaSharp;
using System.Numerics;

namespace SevenWonders.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class BrownCardChildTextureHandler: BaseCardChildTextureHandler<BrownCard>
    {
        public BrownCardChildTextureHandler(IGameEngineReceiver gameEngineReceiver) : base(TextureIdDictionary.GetTextureId("BrownCardHeader"), gameEngineReceiver)
        {
        }

        protected override void HandleCard(BrownCard card, GameObject gameObject)
        {
            Sprite? frontSprite = gameObject.Animations.FirstOrDefault(s => s.Name == "front");
            if (frontSprite is null || frontSprite.Frames.Count == 0)
            {
                return;
            }

            float iconWidthPercent = 0.15f;
            float iconHeightPercent = 0.15f;
            List<ChildObject> childObjects = new List<ChildObject>();

            card.ProducedResources.ForEach(resource =>
            {
                int resourceTextureId = TextureIdDictionary.GetTextureId(resource.GetType().Name);
                childObjects.Add(new ChildTexture
                {
                    TextureId = resourceTextureId,
                    WidthPercent = iconWidthPercent,
                    HeightPercent = iconHeightPercent,
                });
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
