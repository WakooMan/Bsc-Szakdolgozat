using SevenWonders.Game.Engine.SceneObjects;
using System.Numerics;

namespace SevenWonders.Game.Engine.Animations
{
    public class AdjustHighlight : IAnimation
    {
        public bool IsPlaying { get; private set; }

        public AdjustHighlight(GameObject gameObject, Vector2 targetVisualSize, bool highlightValue, float playingDuration)
        {
            IsPlaying = false;
            m_playingDuration = playingDuration;
            m_targetVisualSize = targetVisualSize;
            m_gameObject = gameObject;
            m_highlightValue = highlightValue;
            m_elapsedTime = 0f;
            m_startScale = m_gameObject.VisualSize;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!IsPlaying)
            {
                return;
            }

            m_elapsedTime += deltaTime;
            float t = Math.Clamp(m_elapsedTime / m_playingDuration, 0f, 1f);
            m_gameObject.VisualSize = Vector2.Lerp(m_startScale, m_targetVisualSize, t);

            if (t >= 1.0f)
            {
                m_gameObject.Highlighted = m_highlightValue;
                IsPlaying = false;
                m_elapsedTime = 0f;
            }
        }

        public void Start()
        {
            IsPlaying = true;
        }

        private readonly Vector2 m_startScale;
        private readonly GameObject m_gameObject;
        private readonly Vector2 m_targetVisualSize;
        private readonly float m_playingDuration;
        private readonly bool m_highlightValue;
        private float m_elapsedTime;
    }
}
