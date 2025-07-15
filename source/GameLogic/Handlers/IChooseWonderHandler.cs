using GameLogic.Elements;

namespace GameLogic.Handlers
{
    public interface IChooseWonderHandler
    {
        void ChooseWonder();
        bool WondersChosen { get; }
        ICollection<Player> Players { get; }
    }
}
