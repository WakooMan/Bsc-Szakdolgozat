using SevenWonders.Presenter.Presenters.Factories;

namespace SevenWonders.Presenter.Presenters
{
    public class Presenter : IPresenter
    {
        public Presenter(IPresenterFactory presenterFactory)
        {
            m_presenters = new List<IPresenter>
            {
                presenterFactory.CreateCardPresenter(),
                presenterFactory.CreateWonderPresenter()
            };
        }

        public void Initialize()
        {
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
    }
}
