using SevenWonders.GameEngine.Animations;
using System.Collections.Concurrent;

namespace SevenWonders.GameEngine.Components
{
    public class AnimationManager : IAnimationManager
    {
        public AnimationManager()
        {
            Id = 100;
            Name = nameof(AnimationManager);
            m_activeAnimations = new List<IAnimation>();
            m_animationQueue = new ConcurrentQueue<(List<IAnimation>, TaskCompletionSource?)>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public void Shutdown()
        {
            m_activeAnimations.Clear();
            m_animationQueue.Clear();
            m_activeTcs = null;
        }

        public void Startup()
        {
            m_activeAnimations.Clear();
            m_animationQueue.Clear();
            m_activeTcs = null;
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
                    if (m_animationQueue.TryDequeue(out var animation))
                    {
                        m_activeTcs = animation.Item2;
                        m_activeAnimations.AddRange(animation.Item1);
                        m_activeAnimations.ForEach(animation => animation.Start());
                    }
                }
            }

            foreach (IAnimation animation in m_activeAnimations)
            {
                animation.OnUpdate(deltaTime);
            }

            if (m_activeAnimations.All(animation => !animation.IsPlaying))
            {
                m_activeAnimations.Clear();
                m_activeTcs?.SetResult();
                m_activeTcs = null;
            }
        }

        public void Enqueue(params IAnimation[] animations)
        {
            m_animationQueue.Enqueue(([.. animations], null));
        }

        public Task EnqueueAsync(params IAnimation[] animations)
        {
            var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            m_animationQueue.Enqueue(([.. animations], tcs));
            return tcs.Task;
        }

        private TaskCompletionSource? m_activeTcs;
        private readonly List<IAnimation> m_activeAnimations;
        private readonly ConcurrentQueue<(List<IAnimation>, TaskCompletionSource?)> m_animationQueue;
    }
}
