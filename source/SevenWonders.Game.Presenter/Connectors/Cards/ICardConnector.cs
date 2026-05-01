using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Presenter.Views;

namespace SevenWonders.Game.Presenter.Connectors.Cards
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
