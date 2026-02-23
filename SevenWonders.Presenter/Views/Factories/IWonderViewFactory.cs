namespace SevenWonders.Presenter.Views.Factories
{
    public interface IWonderViewFactory
    {
        IWonderView CreateView(string wonderName);
    }
}
