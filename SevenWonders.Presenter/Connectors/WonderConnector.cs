using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Views;
using SevenWonders.Presenter.Views.Factories;

namespace SevenWonders.Presenter.Connectors
{
    public class WonderConnector : IWonderConnector
    {
        public WonderConnector(ISceneManager sceneManager, IGameElements gameElements, IWonderViewFactory wonderViewFactory)
        {
            m_sceneManager = sceneManager;
            m_wonderList = gameElements.Wonders;
            m_wonderViewFactory = wonderViewFactory;
        }

        public ICollection<GameObject> CreateCenterTargetList()
        {
            return CreateTargetList("centerWonder", 4);
        }

        public ICollection<GameObject> CreatePlayer1TargetList()
        {
            return CreateTargetList("player1Wonder", 4);
        }

        public ICollection<GameObject> CreatePlayer2TargetList()
        {
            return CreateTargetList("player2Wonder", 4);
        }

        public IDictionary<Wonder, IWonderView> CreateWonderConnection()
        {
            Dictionary<Wonder, IWonderView> result = new Dictionary<Wonder, IWonderView>();
            foreach (Wonder wonder in m_wonderList.Wonders)
            {
                result.Add(wonder, m_wonderViewFactory.CreateView(wonder.Name));
            }

            return result;
        }

        private ICollection<GameObject> CreateTargetList(string name, int number)
        {
            List<GameObject> result = new List<GameObject>();
            for (int i = 1; i <= number; i++)
            {
                string targetName = $"{name}{i}";
                GameObject? gameObject = m_sceneManager.GetObjectByName(targetName);
                if (gameObject is null)
                {
                    throw new InvalidOperationException($"GameObject with name {targetName}, does not exist.");
                }
                result.Add(gameObject);
            }

            return result;
        }

        private readonly ISceneManager m_sceneManager;
        private readonly IWonderViewFactory m_wonderViewFactory;
        private readonly IWonderList m_wonderList;
    }
}
