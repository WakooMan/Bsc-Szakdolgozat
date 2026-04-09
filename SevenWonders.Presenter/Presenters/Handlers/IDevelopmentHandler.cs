using SevenWonders.GameEngine;
using SevenWonders.Presenter.Views;

namespace SevenWonders.Presenter.Presenters.Handlers
{
    public interface IDevelopmentHandler
    {
        Task MoveDevelopmentToTarget(IGameObjectView developmentView);
    }
}
