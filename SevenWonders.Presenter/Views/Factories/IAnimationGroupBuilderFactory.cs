using SevenWonders.GameEngine;

namespace SevenWonders.Presenter.Views.Factories
{
    public interface IAnimationGroupBuilderFactory
    {
        IAnimationGroupBuilder Create(GameObject gameObject);
    }
}
