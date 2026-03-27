using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using SevenWonders.GameEngine;
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
                    int i = 0;
                    if (wonder.GoodCost.Any())
                    {
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
                }
            }
        }

        private readonly IGameEngineReceiver m_gameEngineReceiver;
    }
}
