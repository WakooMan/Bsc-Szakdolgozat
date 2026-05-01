using SevenWonders.Game.Presenter.Views;

namespace SevenWonders.Game.Presenter.Presenters.Handlers
{
    public interface IPlayerCardHandler
    {
        Task MoveCardToTarget(IGameObjectView cardView);
    }
}
