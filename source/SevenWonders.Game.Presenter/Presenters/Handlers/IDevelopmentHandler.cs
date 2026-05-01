using SevenWonders.Game.Engine;
using SevenWonders.Game.Presenter.Views;

namespace SevenWonders.Game.Presenter.Presenters.Handlers
{
    public interface IDevelopmentHandler
    {
        Task MoveDevelopmentToTarget(IGameObjectView developmentView);
    }
}
