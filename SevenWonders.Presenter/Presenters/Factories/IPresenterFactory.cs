namespace SevenWonders.Presenter.Presenters.Factories
{
    public interface IPresenterFactory
    {
        void Initialize(bool isMultiplayer);
        IPresenter CreateCardPresenter();
        IPresenter CreateWonderPresenter();
        IPresenter CreatePlayer1Presenter();
        IPresenter CreatePlayer2Presenter();
        IPresenter CreateMilitaryBoardPresenter();
        IPresenter CreateDevelopmentPresenter();
        IPresenter CreateScreenPresenter();
        IPresenter CreateChooseObjectPresenter();
    }
}
