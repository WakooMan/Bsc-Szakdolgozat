using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Presenter.Presenters
{
    public interface IPresenterStore
    {
        void InitializePresenters(IGameOverHandler gameOverHandler);
        void SubscribePresentersToEvents();
    }
}
