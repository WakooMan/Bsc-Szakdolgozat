using GameLogic.Elements.Military;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors.Effects;
using System.Numerics;

namespace SevenWonders.Presenter.Connectors.MilitaryBoard
{
    public class MilitaryTokenChildTextureHandler : IMilitaryTokenChildTextureHandler
    {
        public MilitaryTokenChildTextureHandler(IGameEngineReceiver gameEngineReceiver, IEffectHandler effectHandler, ITextureIdHandler textureIdHandler)
        {
            m_gameEngineReceiver = gameEngineReceiver;
            m_effectHandler = effectHandler;
            m_textureIdHandler = textureIdHandler;
        }

        public void Handle(MilitaryCard militaryCard)
        {
            GameObject gameObject = m_gameEngineReceiver.ReceiveGameObject(militaryCard.Name);
            if (gameObject is not null)
            {
                Sprite? sprite = gameObject.Animations.FirstOrDefault();
                if (sprite is not null)
                {
                    ChildObject? childObject = m_effectHandler.HandleEffect(militaryCard.EnemyLoseMoney, m_textureIdHandler).FirstOrDefault();
                    if (childObject is not null)
                    {
                        childObject.WidthPercent = 0.6f;
                        childObject.HeightPercent = 0.6f;
                        childObject.PositionPercent = new Vector2(0.2f, 0.2f);
                        sprite.AddChildObject(childObject);
                    }
                }
            }
        }

        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IEffectHandler m_effectHandler;
        private readonly ITextureIdHandler m_textureIdHandler;
    }
}
