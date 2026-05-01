using SevenWonders.Game.Engine.Animations;
using System.Threading.Tasks;

namespace SevenWonders.Game.Engine.Components
{
    public interface IAnimationManager: IComponent
    {
        void Enqueue(params IAnimation[] animations);
        Task EnqueueAsync(params IAnimation[] animations);
    }
}
