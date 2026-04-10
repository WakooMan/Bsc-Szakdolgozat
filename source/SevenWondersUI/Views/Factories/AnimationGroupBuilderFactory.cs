using SevenWonders.GameEngine;
using SevenWonders.Presenter.Views;
using SevenWonders.Presenter.Views.Factories;

namespace SevenWondersUI.Views.Factories
{
    public class AnimationGroupBuilderFactory : IAnimationGroupBuilderFactory
    {
        public IAnimationGroupBuilder Create(GameObject gameObject)
        {
            return new AnimationGroupBuilder(gameObject);
        }
    }
}
