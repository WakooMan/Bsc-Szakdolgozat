using GameLogic.Elements.GameCards;
using SevenWonders.GameEngine;
using System.Numerics;

namespace SevenWonders.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class RedCardChildTextureHandler: BaseCardChildTextureHandler<RedCard>
    {
        public RedCardChildTextureHandler(IGameEngineReceiver gameEngineReceiver) : base(TextureIdDictionary.GetTextureId("RedCardHeader"), gameEngineReceiver)
        {
        }

        protected override void HandleCard(RedCard card, GameObject gameObject)
        {
            Sprite? frontSprite = gameObject.Animations.FirstOrDefault(s => s.Name == "front");
            if (frontSprite is null || frontSprite.Frames.Count == 0)
            {
                return;
            }

            int strengthCount = card.GetStrength();
            if (strengthCount == 0)
            {
                return;
            }

            int strengthTextureId = TextureIdDictionary.GetTextureId("Strength");

            const float iconWidthPercent = 0.15f;
            const float iconHeightPercent = 0.15f;

            // Center the group vertically, and start X so the whole group is centered horizontally.
            // PositionPercent (0,0) maps to the top-left of the parent frame (child drawn at top-left corner).
            // Formula to center a child: posX = (1 - iconWidth) / 2, posY = (1 - iconHeight) / 2.
            // For a group of n icons laid out horizontally:
            //   groupWidth = n * iconWidth
            //   groupStartX = (1 - groupWidth) / 2
            //   icon[i].X   = groupStartX + i * iconWidth

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
