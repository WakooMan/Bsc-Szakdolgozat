using SevenWonders.GameEngine;
using System.Numerics;

namespace SevenWonders.Presenter.Views
{
    public interface IWonderView
    {
        void MoveTo(GameObject target);

        void Highlight();

        void Unhighlight();
        void SubscribeClickAtAnimationEnd(Action action);
        void UnsubscribeClick();
    }
}
