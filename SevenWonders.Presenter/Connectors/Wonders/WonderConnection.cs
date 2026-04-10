using SevenWonders.GameEngine;
using SevenWonders.Presenter.Views;

namespace SevenWonders.Presenter.Connectors.Wonders
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
