using SevenWonders.Game.Engine;
using SevenWonders.Game.Presenter.Views;

namespace SevenWonders.Game.Presenter.Connectors.Wonders
{
    public class WonderConnection
    {
        public IGameObjectView GameObjectView { get; }
        public GameObject? WonderTarget { get; set; }
        public GameObject? CardTarget { get; set; }

        public WonderConnection(IGameObjectView gameObjectView)
        {
            GameObjectView = gameObjectView;
        }
    }
}
