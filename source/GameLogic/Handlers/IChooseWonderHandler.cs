using GameLogic.Elements;
using GameLogic.Elements.Wonders;

namespace GameLogic.Handlers
{
    public interface IChooseWonderHandler
    {
        Task ChooseWonder();
        bool WondersChosen { get; }
        void Initialize(ICollection<Player> players, ICollection<Wonder> wonders, IGameContext gameContext);
    }
}
