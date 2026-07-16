using SevenWonders.Game.Engine.SceneObjects;

namespace SevenWonders.Game.Presenter.Views.Factories
{
    public interface IAnimationGroupBuilderFactory
    {
        IAnimationGroupBuilder Create(GameObject gameObject);
    }
}
