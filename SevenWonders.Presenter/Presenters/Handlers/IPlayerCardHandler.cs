using SevenWonders.Presenter.Views;

namespace SevenWonders.Presenter.Presenters.Handlers
{
    public interface IPlayerCardHandler
    {
        Task MoveCardToTarget(IGameObjectView cardView);
    }
}
