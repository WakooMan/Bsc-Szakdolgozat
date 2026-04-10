using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Animations;
using SevenWonders.Presenter.Views;
using System.Numerics;

namespace SevenWondersUI.Views
{
    public class AnimationGroupBuilder : IAnimationGroupBuilder
    {
        public AnimationGroupBuilder(GameObject gameObject)
        {
            m_gameObject = gameObject;
            m_animations = new List<IAnimation>();
        }

        public IAnimationGroupBuilder MoveTo(GameObject target, float playingDuration)
        {
            m_animations.Add(new Movement(m_gameObject, target, playingDuration));
            return this;
        }

        public IAnimationGroupBuilder Flip(int frameNum, float playingDuration)
        {
            m_animations.Add(new CardFlip(m_gameObject, frameNum, playingDuration));
            return this;
        }

        public IAnimationGroupBuilder Highlight(Vector2 targetVisualSize, bool highlightValue, float playingDuration)
        {
            m_animations.Add(new AdjustHighlight(m_gameObject, targetVisualSize, highlightValue, playingDuration));
            return this;
        }

        public IAnimationGroupBuilder Unhighlight(bool highlightValue, float playingDuration)
        {
            m_animations.Add(new AdjustHighlight(m_gameObject, new Vector2(1.0f, 1.0f), highlightValue, playingDuration));
            return this;
        }

        public IAnimation[] GetAnimations()
        {
            return m_animations.ToArray();
        }

        public void Clear()
        {
            m_animations.Clear();
        }

        private readonly GameObject m_gameObject;
        private readonly List<IAnimation> m_animations;
    }
}
