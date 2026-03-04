using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Animations;
using SevenWonders.GameEngine.Components;
using SevenWonders.Presenter.Views;
using System.Numerics;

namespace SevenWondersUI.Views
{
    public class WonderView : IWonderView
    {
        public WonderView(GameObject wonder, IAnimationManager animationManager)
        {
            m_wonder = wonder;
            m_animationManager = animationManager;
        }

        public void MoveTo(GameObject target)
        {
            m_animationManager.Enqueue(new Movement(m_wonder, target, 1.5f), new CardFlip(m_wonder, 0, 1.5f));
        }

        public void Highlight()
        {
            m_animationManager.Enqueue(new AdjustHighlight(m_wonder, new Vector2(1.5f, 1.5f), true, 0.5f));
        }

        public void Unhighlight()
        {
            m_animationManager.Enqueue(new AdjustHighlight(m_wonder, new Vector2(1.0f, 1.0f), false, 0.5f));
        }

        public void SubscribeClickAtAnimationEnd(Action action)
        {
            if (m_touchEvent is null)
            {
                // Wait for animation to end
                m_touchEvent = (args) => action();
                m_wonder.ClickedEvent += m_touchEvent;
            }
        }

        public void UnsubscribeClick()
        {
            if (m_touchEvent is not null)
            {
                m_wonder.ClickedEvent -= m_touchEvent;
                m_touchEvent = null;
            }
        }

        private readonly IAnimationManager m_animationManager;
        private readonly GameObject m_wonder;
        private GameObject.TouchEvent? m_touchEvent;
    }
}
