using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using SevenWonders.GameEngine;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Presenter.Connectors.Wonders.WonderChildTextureHandlers
{
    public class WonderChildTextureHandler : IWonderChildTextureHandler
    {
        public WonderChildTextureHandler(IGameEngineReceiver gameEngineReceiver)
        {
            m_gameEngineReceiver = gameEngineReceiver;
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
                                TextureId = TextureIdDictionary.GetTextureId(good.GetType().Name),
                                WidthPercent = sizePercent,
                                HeightPercent = sizePercent,
                                PositionPercent = new Vector2(0f, 0.9f - i * sizePercent)
                            };
                            frontSprite.AddChildObject(goodTexture);
                            i++;
                        });
                    }

                    List<ChildObject> childObjects = new List<ChildObject>();
                    //wonder.Effects.ForEach(effect =>
                    //{
                    //    if (m_effectHandlers.TryGetValue(effect.GetType(), out Func<Effect, ICollection<ChildObject>>? handler))
                    //    {
                    //        childObjects.AddRange(handler(effect));
                    //    }
                    //});

                    //float totalWidthPercent = childObjects.Sum(co => co.WidthPercent);
                    //float groupStartX = (1f - totalWidthPercent) / 2f;
                    //float centeredY = (0.2f - childObjects.First().HeightPercent);
                    //float currentX = groupStartX;
                    //foreach (ChildObject childObject in childObjects)
                    //{
                    //    childObject.PositionPercent = new Vector2(currentX, centeredY);
                    //    frontSprite.AddChildObject(childObject);
                    //    currentX += childObject.WidthPercent;
                    //}

                    ChildTextLabel childTextLabel = new ChildTextLabel
                    {
                        TextLabel = new TextLabel
                        {
                            Text = wonder.Name,
                            TextColor = SKColors.Wheat,
                            FontSize = 12,
                            BackgroundTextureId = TextureIdDictionary.GetTextureId("CardNameBackground"),
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
    }
}
