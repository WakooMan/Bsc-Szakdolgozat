using SevenWonders.Game.Engine.ChildObjects;

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
        bool GetVisible();
        int GetAnimationIndex();
        void AddChildObject(ChildObject childObject);
        T? GetChildObject<T>(string name) where T : ChildObject;
    }
}
