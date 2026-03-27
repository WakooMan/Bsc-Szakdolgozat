namespace SevenWonders.Presenter.Views
{
    public interface IGameObjectView
    {
        IAnimationGroupBuilder GetAnimationGroupBuilder();
        Task Execute();
        void IncreaseZIndex();
        void DecreaseZIndex();
    }
}
