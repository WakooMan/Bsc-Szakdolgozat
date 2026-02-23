using GameLogic.Elements.Wonders;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Views;

namespace SevenWonders.Presenter.Connectors
{
    public interface IWonderConnector
    {
        IDictionary<Wonder, IWonderView> CreateWonderConnection();

        ICollection<GameObject> CreatePlayer1TargetList();

        ICollection<GameObject> CreatePlayer2TargetList();

        ICollection<GameObject> CreateCenterTargetList();
    }
}
