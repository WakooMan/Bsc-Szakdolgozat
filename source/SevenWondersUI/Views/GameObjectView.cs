using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Animations;
using SevenWonders.GameEngine.Components;
using SevenWonders.Presenter.Views;
using SevenWonders.Presenter.Views.Factories;
using System.Numerics;

namespace SevenWondersUI.Views
{
    public class GameObjectView : IGameObjectView
    {
        public GameObjectView(GameObject wonder, IAnimationManager animationManager, IAnimationGroupBuilderFactory animationGroupBuilderFactory)
        {
            m_gameObject = wonder;
            m_animationManager = animationManager;
            m_groupBuilder = animationGroupBuilderFactory.Create(wonder);
        }

        public void SubscribeClickAtAnimationEnd(Action action)
        {
            if (m_touchEvent is null)
            {
                // Wait for animation to end
                m_touchEvent = (args) => action();
                m_gameObject.ClickedEvent += m_touchEvent;
            }
        }

        public void UnsubscribeClick()
        {
            if (m_touchEvent is not null)
            {
                m_gameObject.ClickedEvent -= m_touchEvent;
                m_touchEvent = null;
            }
        }

        public IAnimationGroupBuilder GetAnimationGroupBuilder()
        {
            return m_groupBuilder;
        }

        public void Execute()
        {
            m_animationManager.Enqueue(m_groupBuilder.GetAnimations());
            m_groupBuilder.Clear();
        }

        public void IncreaseZIndex()
        {
            m_gameObject.ZIndex++;
        }

        public void DecreaseZIndex()
        {
            m_gameObject.ZIndex--;
        }

        private readonly IAnimationManager m_animationManager;
        private readonly IAnimationGroupBuilder m_groupBuilder;
        private readonly GameObject m_gameObject;
        private GameObject.TouchEvent? m_touchEvent;
    }
}
