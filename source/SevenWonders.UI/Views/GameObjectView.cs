using SevenWonders.Game.Engine.Components;
using SevenWonders.Game.Engine.SceneObjects;
using SevenWonders.Game.Presenter.Views;
using SevenWonders.Game.Presenter.Views.Factories;

namespace SevenWonders.UI.Views
{
    public class GameObjectView : IGameObjectView
    {
        public string Name => m_gameObject.Name;
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

        public int FindAnimationIndexByName(string name)
        {
            return m_gameObject.Animations.FindIndex(anim => anim.Name.ToLower() == name.ToLower());
        }

        private readonly IAnimationManager m_animationManager;
        private readonly IAnimationGroupBuilder m_groupBuilder;
        private readonly GameObject m_gameObject;
    }
}
