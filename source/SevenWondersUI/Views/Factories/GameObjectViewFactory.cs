using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Components;
using SevenWonders.Presenter.Connectors;
using SevenWonders.Presenter.Views;
using SevenWonders.Presenter.Views.Factories;

namespace SevenWondersUI.Views.Factories
{
    public class GameObjectViewFactory : IGameObjectViewFactory
    {
        public GameObjectViewFactory(ISceneManager sceneManager, IAnimationManager animationManager, IGameObjectReceiver gameObjectReceiver)
        {
            m_gameObjectReceiver = gameObjectReceiver;
            m_sceneManager = sceneManager;
            m_animationManager = animationManager;
        }

        public IGameObjectView CreateView(string wonderName)
        {
            GameObject gameObject = m_gameObjectReceiver.ReceiveGameObject(wonderName);
            return new GameObjectView(gameObject, m_animationManager);
        }

        private readonly ISceneManager m_sceneManager;
        private readonly IAnimationManager m_animationManager;
        private readonly IGameObjectReceiver m_gameObjectReceiver;
    }
}
