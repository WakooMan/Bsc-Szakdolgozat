namespace SevenWonders.Presenter.Views.Factories
{
    public interface IGameObjectViewFactory
    {
        IGameObjectView CreateView(string wonderName);
    }
}
