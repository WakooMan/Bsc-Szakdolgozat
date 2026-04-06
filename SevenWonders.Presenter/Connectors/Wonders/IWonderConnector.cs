using GameLogic.Elements.Wonders;

namespace SevenWonders.Presenter.Connectors.Wonders
{
    public interface IWonderConnector
    {
        IDictionary<Wonder, WonderConnection> ReceiveWonderConnection();
    }
}
