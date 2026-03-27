namespace SevenWonders.Presenter.Views
{
    public interface IGameObjectView
    {
        IAnimationGroupBuilder GetAnimationGroupBuilder();
        void Execute();
        void IncreaseZIndex();
        void DecreaseZIndex();
    }
}
