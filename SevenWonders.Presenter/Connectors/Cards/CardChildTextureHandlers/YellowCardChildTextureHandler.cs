using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using Microsoft.Maui.Controls;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors.Effects;
using SkiaSharp;
using System.Numerics;
using System.Runtime.CompilerServices;
using Effect = GameLogic.Elements.Effects.Effect;

namespace SevenWonders.Presenter.Connectors.Cards.CardChildTextureHandlers
{
    public class YellowCardChildTextureHandler: BaseCardChildTextureHandler<YellowCard>
    {
        public YellowCardChildTextureHandler(IGameEngineReceiver gameEngineReceiver, IEffectHandler effectHandler, ITextureIdHandler textureIdHandler) : base("YellowCard", gameEngineReceiver, textureIdHandler)
        {
            m_effectHandler = effectHandler;
        }

        protected override void HandleCard(YellowCard card, GameObject gameObject)
        {
            List<ChildObject> childObjects = new List<ChildObject>();
            card.Effects.ForEach(effect =>
            {
               childObjects.AddRange(m_effectHandler.HandleEffect(effect, m_textureIdHandler));
            });

            Sprite? frontSprite = gameObject.Animations.FirstOrDefault(s => s.Name == "front");
            if (frontSprite is not null && frontSprite.Frames.Count > 0)
            {
                float totalWidthPercent = childObjects.Sum(co => co.WidthPercent);
                float groupStartX = (1f - totalWidthPercent) / 2f;
                float centeredY = (0.2f - childObjects.First().HeightPercent);
                float currentX = groupStartX;
                foreach (ChildObject childObject in childObjects)
                {
                    childObject.PositionPercent = new Vector2(currentX, centeredY);
                    frontSprite.AddChildObject(childObject);
                    currentX += childObject.WidthPercent;
                }
            }
        }

        private readonly IEffectHandler m_effectHandler;
    }
}
