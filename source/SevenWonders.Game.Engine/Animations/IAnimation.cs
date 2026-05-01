namespace SevenWonders.Game.Engine.Animations
{
    public interface IAnimation
    {
        bool IsPlaying { get; }
        void OnUpdate(float deltaTime);
        void Start();
    }
}
