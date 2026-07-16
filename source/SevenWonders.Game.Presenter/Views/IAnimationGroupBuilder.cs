using SevenWonders.Game.Engine.Animations;
using SevenWonders.Game.Engine.SceneObjects;
using System.Numerics;

namespace SevenWonders.Game.Presenter.Views
{
    public interface IAnimationGroupBuilder
    {
        IAnimationGroupBuilder MoveTo(GameObject target, float playingDuration);
        IAnimationGroupBuilder Flip(string frameName, float playingDuration);
        IAnimationGroupBuilder Highlight(Vector2 targetVisualSize, bool highlightValue, float playingDuration);
        IAnimationGroupBuilder Unhighlight(bool highlightValue, float playingDuration);
        IAnimation[] GetAnimations();
        void Clear();
    }
}
