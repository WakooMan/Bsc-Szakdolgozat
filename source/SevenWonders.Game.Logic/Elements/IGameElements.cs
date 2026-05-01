using SevenWonders.Game.Logic.Elements.Developments;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Wonders;

namespace SevenWonders.Game.Logic.Elements
{
    public interface IGameElements
    {
        ICardList? Cards { get; }
        IWonderList? Wonders { get; }
        IDevelopmentList? Developments { get; }

        public void ResetElements();
    }
}
