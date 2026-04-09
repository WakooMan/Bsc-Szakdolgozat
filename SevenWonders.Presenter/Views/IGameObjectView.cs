namespace SevenWonders.Presenter.Views
{
    public interface IGameObjectView
    {
        bool IsDimmed { get; }
        IAnimationGroupBuilder GetAnimationGroupBuilder();
        Task Execute();

        void SetVisible(bool visible);
    }
}
