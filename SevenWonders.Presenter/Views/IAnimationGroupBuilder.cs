using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Animations;
using System.Numerics;

namespace SevenWonders.Presenter.Views
{
    public interface IAnimationGroupBuilder
    {
        IAnimationGroupBuilder MoveTo(GameObject target, float playingDuration);
        IAnimationGroupBuilder Flip(int frameNum, float playingDuration);
        IAnimationGroupBuilder Highlight(Vector2 targetVisualSize, bool highlightValue, float playingDuration);
        IAnimationGroupBuilder Unhighlight(bool highlightValue, float playingDuration);
        IAnimation[] GetAnimations();
        void Clear();
    }
}
