using SevenWonders.Game.Engine.SceneObjects;
using System.Numerics;

namespace SevenWonders.Game.Engine.Animations
{
    public class Movement : IAnimation
    {
        public bool IsPlaying { get; private set; }

        public Movement(GameObject gameObject, GameObject target, float playingDuration)
        {
            m_gameObject = gameObject;
            m_target = target;
            m_playingDuration = playingDuration;
            IsPlaying = false;
            m_elapsedTime = 0f;
        }

        public void Start()
        {
            IsPlaying = true;
            m_gameObject.ZIndex = m_target.ZIndex;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!IsPlaying)
            {
                return;
            }

            m_elapsedTime += deltaTime;
            float timeLeft = m_playingDuration - m_elapsedTime;

            if (timeLeft <= 0 || m_elapsedTime >= m_playingDuration)
            {
                m_gameObject.Position = m_target.Position;
                m_gameObject.Rotation = m_target.Rotation;
                IsPlaying = false;
                return;
            }

            float t = Math.Clamp(m_elapsedTime / m_playingDuration, 0f, 1f);
            m_gameObject.Position = Vector2.Lerp(m_gameObject.Position, m_target.Position, t);
            m_gameObject.Rotation = LerpRotation(m_gameObject.Rotation, m_target.Rotation, t);
        }

        private float LerpRotation(float current, float target, float t)
        {
            float diff = ((target - current + 180) % 360);
            if (diff < 0) diff += 360;
            diff -= 180;

            float next = (current + diff * t) % 360;
            return next < 0 ? next + 360 : next;
        }

        private float m_elapsedTime;
        private readonly GameObject m_gameObject;
        private readonly GameObject m_target;
        private readonly float m_playingDuration;
    }
}