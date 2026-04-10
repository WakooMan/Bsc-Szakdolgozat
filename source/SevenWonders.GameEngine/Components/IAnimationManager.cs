using SevenWonders.GameEngine.Animations;
using System.Threading.Tasks;

namespace SevenWonders.GameEngine.Components
{
    public interface IAnimationManager: IComponent
    {
        void Enqueue(params IAnimation[] animations);
        Task EnqueueAsync(params IAnimation[] animations);
    }
}
