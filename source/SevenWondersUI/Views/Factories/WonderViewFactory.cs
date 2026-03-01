using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Components;
using SevenWonders.Presenter.Views;
using SevenWonders.Presenter.Views.Factories;

namespace SevenWondersUI.Views.Factories
{
    public class WonderViewFactory : IWonderViewFactory
    {
        public WonderViewFactory(ISceneManager sceneManager, IAnimationManager animationManager)
        {
            m_sceneManager = sceneManager;
            m_animationManager = animationManager;
        }

        public IWonderView CreateView(string wonderName)
        {
            GameObject? gameObject = m_sceneManager.GetObjectByName(wonderName);
            if (gameObject is null)
            {
                throw new InvalidOperationException($"Did not find the game object with name: {wonderName}");
            }
            return new WonderView(gameObject, m_animationManager);
        }

        private readonly ISceneManager m_sceneManager;
        private readonly IAnimationManager m_animationManager;
    }
}
