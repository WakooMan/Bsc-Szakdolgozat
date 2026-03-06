using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.GameCards;

namespace SevenWonders.Presenter.Presenters
{
    public interface ICardPresenter
    {
        void MoveToPlayer(Player player, Card card);
        void MoveToActionLocation(Card card);
        //void MoveToCardDeck(Card card);
        void MoveToDropCardDeck(Card card);

        void MoveToCenter(Card card, AgesEnum age);
        void Initialize();
    }
}
