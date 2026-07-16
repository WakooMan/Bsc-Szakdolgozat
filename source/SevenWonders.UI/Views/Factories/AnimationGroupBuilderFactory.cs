using SevenWonders.Game.Engine.SceneObjects;
using SevenWonders.Game.Presenter.Views;
using SevenWonders.Game.Presenter.Views.Factories;

namespace SevenWonders.UI.Views.Factories
{
    public class AnimationGroupBuilderFactory : IAnimationGroupBuilderFactory
    {
        public IAnimationGroupBuilder Create(GameObject gameObject)
        {
            return new AnimationGroupBuilder(gameObject);
        }
    }
}
