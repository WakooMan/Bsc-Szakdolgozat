namespace SevenWonders.Presenter.Presenters.Factories
{
    public interface IPresenterFactory
    {
        IPresenter CreateCardPresenter();
        IPresenter CreateWonderPresenter();
        IPresenter CreatePlayer1Presenter();
        IPresenter CreatePlayer2Presenter();
        IPresenter CreateMilitaryBoardPresenter();
        IPresenter CreateDevelopmentPresenter();
    }
}
