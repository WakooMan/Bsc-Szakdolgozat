using GameLogic.Elements.Wonders;
using SevenWonders.Presenter.Views;

namespace SevenWonders.Presenter.Connectors
{
    public interface IWonderConnector
    {
        IDictionary<Wonder, IGameObjectView> ReceiveWonderConnection();
    }
}
