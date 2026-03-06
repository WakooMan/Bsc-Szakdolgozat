using SevenWonders.GameEngine;

namespace SevenWonders.Presenter.Views
{
    public interface IGameObjectView
    {
        void MoveTo(GameObject target);

        void Highlight();

        void Unhighlight();
        void SubscribeClickAtAnimationEnd(Action action);
        void UnsubscribeClick();
    }
}
