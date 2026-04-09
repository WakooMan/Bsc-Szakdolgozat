namespace SevenWonders.Presenter.Views
{
    public interface IGameObjectView
    {
        string Name { get; }
        bool IsDimmed { get; }
        IAnimationGroupBuilder GetAnimationGroupBuilder();
        Task Execute();

        void SetVisible(bool visible);
    }
}
