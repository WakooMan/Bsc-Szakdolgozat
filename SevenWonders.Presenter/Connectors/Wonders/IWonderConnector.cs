using GameLogic.Elements.Wonders;
using SevenWonders.Presenter.Views;

namespace SevenWonders.Presenter.Connectors.Wonders
{
    public interface IWonderConnector
    {
        IDictionary<Wonder, IGameObjectView> ReceiveWonderConnection();
    }
}
