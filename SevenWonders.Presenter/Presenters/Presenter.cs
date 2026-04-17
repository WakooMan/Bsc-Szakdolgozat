using SevenWonders.Presenter.Presenters.Factories;

namespace SevenWonders.Presenter.Presenters
{
    public class Presenter : IPresenter
    {
        public Presenter(IPresenterFactory presenterFactory)
        {
            m_presenterFactory = presenterFactory;
            m_presenters = new List<IPresenter>();
        }

        public void Initialize()
        {
            m_presenters.Clear();
            m_presenters.AddRange(
            [
                m_presenterFactory.CreateCardPresenter(),
                m_presenterFactory.CreateWonderPresenter(),
                m_presenterFactory.CreatePlayer1Presenter(),
                m_presenterFactory.CreatePlayer2Presenter(),
                m_presenterFactory.CreateMilitaryBoardPresenter(),
                m_presenterFactory.CreateDevelopmentPresenter(),
                m_presenterFactory.CreateScreenPresenter(),
                m_presenterFactory.CreateChooseObjectPresenter()
            ]);

            foreach (var presenter in m_presenters)
            {
                presenter.Initialize();
            }
        }

        public void SubscribeToEvents()
        {
            foreach (var presenter in m_presenters)
            {
                presenter.SubscribeToEvents();
            }   
        }

        private readonly List<IPresenter> m_presenters;
        private readonly IPresenterFactory m_presenterFactory;
    }
}
