using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using SevenWonders.GameEngine;
using SkiaSharp;
using System.Numerics;

namespace SevenWonders.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class BlueCardChildTextureHandler : BaseCardChildTextureHandler<BlueCard>
    {
        public BlueCardChildTextureHandler(IGameEngineReceiver gameEngineReceiver, ITextureIdHandler textureIdHandler) : base("BlueCard", gameEngineReceiver, textureIdHandler)
        {
        }

        protected override void HandleCard(BlueCard card, GameObject gameObject)
        {
            Sprite? frontSprite = gameObject.Animations.FirstOrDefault(s => s.Name == "front");
            if (frontSprite is null || frontSprite.Frames.Count == 0)
            {
                return;
            }

            float iconWidthPercent = 0.15f;
            float iconHeightPercent = 0.15f;
            List<ChildObject> childObjects = new List<ChildObject>();

            if (card.Point.Points > 0)
            {
                int victoryPointsTextureId = m_textureIdHandler.GetTextureId(nameof(VictoryPoints));
                childObjects.Add(new ChildTextLabel
                {
                    TextLabel = new TextLabel()
                    {
                        BackgroundTextureId = victoryPointsTextureId,
                        Text = card.Point.Points.ToString(),
                        TextColor = SKColors.AntiqueWhite,
                        FontSize = 13,
                        Visible = true,
                    },
                    WidthPercent = iconWidthPercent,
                    HeightPercent = iconHeightPercent,
                });
            }

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
