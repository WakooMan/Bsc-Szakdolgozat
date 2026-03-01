using SevenWonders.GameEngine.Animations;

namespace SevenWonders.GameEngine.Components
{
    public interface IAnimationManager: IComponent
    {
        void Enqueue(params IAnimation[] animations);
    }
}
