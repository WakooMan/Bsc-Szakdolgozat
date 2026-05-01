using SevenWonders.Game.Engine;

namespace SevenWonders.Game.Presenter.Views.Factories
{
    public interface IAnimationGroupBuilderFactory
    {
        IAnimationGroupBuilder Create(GameObject gameObject);
    }
}
