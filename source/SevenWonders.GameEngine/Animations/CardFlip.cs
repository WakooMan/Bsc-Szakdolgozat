using System.Numerics;

namespace SevenWonders.GameEngine.Animations
{
    public class CardFlip : IAnimation
    {
        public bool IsPlaying { get; private set; }

        public CardFlip(GameObject gameObject, int spriteNumber, float playingDuration)
        {
            m_gameObject = gameObject;
            m_spriteNumber = spriteNumber;
            m_playingDuration = playingDuration;
            IsPlaying = false;
            m_elapsedTime = 0f;
            m_spriteSwapped = false;
        }

        public void Start()
        {
            IsPlaying = true;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!IsPlaying)
            {
                return;
            }

            m_elapsedTime += deltaTime;
            float t = Math.Clamp(m_elapsedTime / m_playingDuration, 0f, 1f);

            float newScaleX = t < 0.5f ? 1.0f - (2.0f * t) : (2.0f * t) - 1.0f;
            m_gameObject.FlipMultiplier = new Vector2(newScaleX, m_gameObject.FlipMultiplier.Y);

            if (!m_spriteSwapped && t >= 0.5f)
            {
                m_gameObject.CurrentAnim = m_spriteNumber;
                m_spriteSwapped = true;
            }

            if (t >= 1.0f)
            {
                m_gameObject.FlipMultiplier = new Vector2(1f, m_gameObject.FlipMultiplier.Y);
                IsPlaying = false;
            }
        }

        private float m_elapsedTime;
        private bool m_spriteSwapped;
        private readonly float m_playingDuration;
        private readonly int m_spriteNumber;
        private readonly GameObject m_gameObject;
    }
}