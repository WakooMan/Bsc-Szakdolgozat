using GameLogic.Elements.GameCards;
using SevenWonders.Presenter.Views;

namespace SevenWonders.Presenter.Connectors.Cards
{
    public interface ICardConnector
    {
        //IDictionary<Type, GameObject> ReceivePlayer1CardTargets();
        //IDictionary<Type, GameObject> ReceivePlayer2CardTargets();
        //GameObject ReceiveDroppedCardDeck();
        //GameObject ReceiveCardDeck();

        //GameObject ReceiveCardAction();

        //ICollection<GameObject> ReceiveFirstAgeCardLocations();
        //ICollection<GameObject> ReceiveSecondAgeCardLocations();
        //ICollection<GameObject> ReceiveThirdAgeCardLocations();

        IDictionary<Card, IGameObjectView> ReceiveCardConnection();

    }
}
