using SevenWonders.Game.Logic.Elements.Wonders;

namespace SevenWonders.Game.Presenter.Connectors.Wonders
{
    public interface IWonderConnector
    {
        IDictionary<Wonder, WonderConnection> ReceiveWonderConnection();
    }
}
