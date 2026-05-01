using SevenWonders.Game.Engine;

namespace SevenWonders.Game.Presenter.Connectors
{
    public class GameObjectReceiver : IGameObjectReceiver
    {
        public GameObjectReceiver(ISceneManager sceneManager)
        {
            m_sceneManager = sceneManager;
        }

        public GameObject ReceiveGameObject(string name)
        {
            GameObject? gameObject = m_sceneManager.GetObjectByName(name);
            if (gameObject is null)
            {
                throw new InvalidOperationException($"GameObject with name {name} does not exist.");
            }
            return gameObject;
        }

        public ICollection<GameObject> ReceiveGameObjects(string name, int number)
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
    }
}
