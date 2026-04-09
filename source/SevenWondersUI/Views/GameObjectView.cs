using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Components;
using SevenWonders.Presenter.Views;
using SevenWonders.Presenter.Views.Factories;

namespace SevenWondersUI.Views
{
    public class GameObjectView : IGameObjectView
    {
        public bool IsDimmed =>m_gameObject.Dimmed;

        public GameObjectView(GameObject wonder, IAnimationManager animationManager, IAnimationGroupBuilderFactory animationGroupBuilderFactory)
        {
            m_gameObject = wonder;
            m_animationManager = animationManager;
            m_groupBuilder = animationGroupBuilderFactory.Create(wonder);
        }

        public IAnimationGroupBuilder GetAnimationGroupBuilder()
        {
            return m_groupBuilder;
        }

        public async Task Execute()
        {
            await m_animationManager.EnqueueAsync(m_groupBuilder.GetAnimations());
            m_groupBuilder.Clear();
        }

        public void SetVisible(bool visible)
        {
            m_gameObject.Visible = visible;
        }

        private readonly IAnimationManager m_animationManager;
        private readonly IAnimationGroupBuilder m_groupBuilder;
        private readonly GameObject m_gameObject;
    }
}
