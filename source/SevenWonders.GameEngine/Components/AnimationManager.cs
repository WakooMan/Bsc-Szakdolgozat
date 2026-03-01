using SevenWonders.GameEngine.Animations;

namespace SevenWonders.GameEngine.Components
{
    public class AnimationManager : IAnimationManager
    {
        public AnimationManager()
        {
            Id = 100;
            Name = nameof(AnimationManager);
            m_activeAnimations = new List<IAnimation>();
            m_animationQueue = new Queue<List<IAnimation>>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public void Shutdown()
        {
            m_activeAnimations.Clear();
            m_animationQueue.Clear();
        }

        public void Startup()
        {
            m_activeAnimations.Clear();
            m_animationQueue.Clear();
        }

        public void Update(float deltaTime)
        {
            if (m_activeAnimations.Count <= 0)
            {
                if (m_animationQueue.Count <= 0)
                {
                    return;
                }
                else
                {
                    m_activeAnimations.AddRange(m_animationQueue.Dequeue());
                    m_activeAnimations.ForEach(animation => animation.Start());
                }
            }

            foreach (IAnimation animation in m_activeAnimations)
            {
                animation.OnUpdate(deltaTime);
            }

            if (m_activeAnimations.All(animation => !animation.IsPlaying))
            {
                m_activeAnimations.Clear();
            }
        }

        public void Enqueue(params IAnimation[] animations)
        {
            m_animationQueue.Enqueue([.. animations]);
        }

        private readonly List<IAnimation> m_activeAnimations;
        private readonly Queue<List<IAnimation>> m_animationQueue;
    }
}
