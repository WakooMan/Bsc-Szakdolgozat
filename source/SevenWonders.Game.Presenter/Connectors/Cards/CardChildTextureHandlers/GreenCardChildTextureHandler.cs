using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Elements.GameCards;
using SkiaSharp;
using System.Numerics;
using SevenWonders.Game.Engine.SceneObjects;
using SevenWonders.Game.Engine.ChildObjects;

namespace SevenWonders.Game.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class GreenCardChildTextureHandler : BaseCardChildTextureHandler<GreenCard>
    {
        public GreenCardChildTextureHandler(IGameEngineReceiver gameEngineReceiver, ITextureIdHandler textureIdHandler) : base("GreenCard", gameEngineReceiver, textureIdHandler)
        {
        }

        protected override void HandleCard(GreenCard card, GameObject gameObject)
        {
            Sprite? frontSprite = gameObject.Animations.FirstOrDefault(s => s.Name == "front");
            if (frontSprite is null || frontSprite.Frames.Count == 0)
            {
                return;
            }

            float iconWidthPercent = 0.22f;
            float iconHeightPercent = 0.22f;
            List<ChildObject> childObjects = new List<ChildObject>();

            int disciplineTextureId = m_textureIdHandler.GetTextureId(card.Discipline.GetType().Name);
            childObjects.Add(new ChildTexture
            {
                TextureId = disciplineTextureId,
                WidthPercent = iconWidthPercent,
                HeightPercent = iconHeightPercent,
            });

            if (card.Point.Points > 0)
            {
                int victoryPointsTextureId = m_textureIdHandler.GetTextureId(nameof(VictoryPoints));
                childObjects.Add(new ChildTextLabel
                {
                    TextLabel = new TextLabel()
                    {
                        Visible = true,
                        TextProperties = new TextProperties()
                        {
                            Text = card.Point.Points.ToString(),
                            TextColor = SKColors.AntiqueWhite,
                            FontSize = 13,
                        }
                    },
                    BackgroundTextureId = victoryPointsTextureId,
                    WidthPercent = iconWidthPercent,
                    HeightPercent = iconHeightPercent,
                });
            }

            float groupStartX = (1f - childObjects.Count * iconWidthPercent) / 2f;
            float centeredY = (0.23f - iconHeightPercent);

            for (int i = 0; i < childObjects.Count; i++)
            {
                float posX = groupStartX + i * iconWidthPercent;
                childObjects[i].PositionPercent = new Vector2(posX, centeredY);
                frontSprite.AddChildObject(childObjects[i]);
            }
        }
    }
}