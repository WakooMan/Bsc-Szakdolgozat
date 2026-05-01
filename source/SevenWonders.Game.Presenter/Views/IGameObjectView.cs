namespace SevenWonders.Game.Presenter.Views
{
    public interface IGameObjectView
    {
        string Name { get; }
        bool IsDimmed { get; }
        IAnimationGroupBuilder GetAnimationGroupBuilder();
        Task Execute();
        int FindAnimationIndexByName(string name);
        void SetVisible(bool visible);
    }
}
