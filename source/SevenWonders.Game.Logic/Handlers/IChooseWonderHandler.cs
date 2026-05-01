using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Wonders;

namespace SevenWonders.Game.Logic.Handlers
{
    public interface IChooseWonderHandler
    {
        void ChooseWonder();
        bool WondersChosen { get; }
        void Initialize(ICollection<Player> players, ICollection<Wonder> wonders, IGameContext gameContext);
    }
}
