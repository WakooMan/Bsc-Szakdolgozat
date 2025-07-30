using GameLogic.Elements;
using GameLogic.Elements.Wonders;

namespace GameLogic.Handlers
{
    public interface IChooseWonderHandler
    {
        void ChooseWonder();
        bool WondersChosen { get; }
        void Initialize(ICollection<Player> players, ICollection<Wonder> wonders);
    }
}
