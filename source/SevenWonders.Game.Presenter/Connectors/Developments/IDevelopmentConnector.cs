using SevenWonders.Game.Logic.Elements.Modifiers;
using SevenWonders.Game.Presenter.Views;

namespace SevenWonders.Game.Presenter.Connectors.Developments
{
    public interface IDevelopmentConnector
    {
        IDictionary<Development, IGameObjectView> ReceiveDevelopmentConnection();
    }
}
