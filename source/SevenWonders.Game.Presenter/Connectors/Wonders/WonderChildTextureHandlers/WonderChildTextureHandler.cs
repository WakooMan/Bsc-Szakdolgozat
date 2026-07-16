using SevenWonders.Game.Logic.Elements.Wonders;
using SevenWonders.Game.Presenter.Connectors.Effects;
using SkiaSharp;
using System.Numerics;
using SevenWonders.Game.Engine.SceneObjects;
using SevenWonders.Game.Engine.ChildObjects;

namespace SevenWonders.Game.Presenter.Connectors.Wonders.WonderChildTextureHandlers
{
    public class WonderChildTextureHandler : IWonderChildTextureHandler
    {
        public WonderChildTextureHandler(IGameEngineReceiver gameEngineReceiver, IEffectHandler effectHandler, ITextureIdHandler textureIdHandler)
        {
            m_gameEngineReceiver = gameEngineReceiver;
            m_effectHandler = effectHandler;
            m_textureIdHandler = textureIdHandler;
        }

        public void Handle(Wonder wonder)
        {
            GameObject? gameObject = m_gameEngineReceiver.ReceiveGameObject(wonder.Name);
            if (gameObject is not null)
            {
                Sprite? frontSprite = gameObject.Animations.FirstOrDefault(s => s.Name == "front");
                if (frontSprite is not null && frontSprite.Frames.Count > 0)
                {
                    if (wonder.GoodCost.Any())
                    {
                        int i = 0;
                        wonder.GoodCost.ForEach(good =>
                        {
                            float sizePercent = 0.15f;
                            ChildTexture goodTexture = new ChildTexture
                            {
                                TextureId = m_textureIdHandler.GetTextureId(good.GetType().Name),
                                WidthPercent = sizePercent,
                                HeightPercent = sizePercent,
                                PositionPercent = new Vector2(0f, 0.9f - i * sizePercent)
                            };
                            frontSprite.AddChildObject(goodTexture);
                            i++;
                        });
                    }

                    List<ChildObject> childObjects = new List<ChildObject>();
                    wonder.Effects.ForEach(effect =>
                    {
                        childObjects.AddRange(m_effectHandler.HandleEffect(effect, m_textureIdHandler));
                    });

                    if (childObjects.Any())
                    {
                        float totalHeightPercent = childObjects.Sum(co => co.HeightPercent);
                        float groupStartY = (1f - totalHeightPercent) / 2f;
                        float locationX = (1.0f - childObjects.First().WidthPercent);
                        float currentY = groupStartY;
                        foreach (ChildObject childObject in childObjects)
                        {
                            childObject.PositionPercent = new Vector2(locationX, currentY);
                            frontSprite.AddChildObject(childObject);
                            currentY += childObject.HeightPercent;
                        }
                    }

                    ChildTextLabel childTextLabel = new ChildTextLabel
                    {
                        TextLabel = new TextLabel
                        {
                            Text = wonder.Name,
                            TextColor = SKColors.Wheat,
                            FontSize = 12,
                            BackgroundTextureId = m_textureIdHandler.GetTextureId("CardNameBackground"),
                            Visible = true
                        },
                        WidthPercent = 0.6f,
                        HeightPercent = 0.1f,
                        PositionPercent = new Vector2(0.2f, 0.8f)
                    };

                    frontSprite.AddChildObject(childTextLabel);
                }
            }
        }


        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IEffectHandler m_effectHandler;
        private readonly ITextureIdHandler m_textureIdHandler;
    }
}
