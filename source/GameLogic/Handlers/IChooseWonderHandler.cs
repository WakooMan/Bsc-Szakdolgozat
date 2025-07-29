using GameLogic.Elements;

namespace GameLogic.Handlers
{
    public interface IChooseWonderHandler
    {
        void ChooseWonder();
        bool WondersChosen { get; }
        void SetPlayers(ICollection<Player> players);
    }
}
