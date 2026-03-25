using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.GameCards;

namespace SevenWonders.Presenter.Presenters
{
    public interface ICardPresenter
    {
        delegate void CardPresenterDelegate(Card card);
        delegate void DecisionDelegate();
        event CardPresenterDelegate CardChosen;
        event DecisionDelegate BuildCardChosen;
        event DecisionDelegate SellCardChosen;
        event DecisionDelegate UnpickCardChosen;
        event DecisionDelegate BuildWonderChosen;
        void MoveToPlayer(Player player, Card card);
        void MoveToActionLocation(Card card);
        //void MoveToCardDeck(Card card);
        void MoveToDropCardDeck(Card card);

        void MoveToCenter(Card card, bool hidden, string nodeName);
        void MoveBackToCenter(Card card, string nodeName);
        void Initialize();
    }
}
