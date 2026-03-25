namespace SevenWonders.Presenter.Views
{
    public interface IGameObjectView
    {
        IAnimationGroupBuilder GetAnimationGroupBuilder();

        void SubscribeClickAtAnimationEnd(Action action);
        void UnsubscribeClick();

        void Execute();
        void IncreaseZIndex();
        void DecreaseZIndex();
    }
}
