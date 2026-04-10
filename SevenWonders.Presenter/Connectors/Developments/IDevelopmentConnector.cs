using GameLogic.Elements.Modifiers;
using SevenWonders.Presenter.Views;

namespace SevenWonders.Presenter.Connectors.Developments
{
    public interface IDevelopmentConnector
    {
        IDictionary<Development, IGameObjectView> ReceiveDevelopmentConnection();
    }
}
