namespace SevenWonders.Presenter.Presenters.Factories
{
    public interface IPresenterFactory
    {
        IPresenter CreateCardPresenter();
        IPresenter CreateWonderPresenter();
    }
}
