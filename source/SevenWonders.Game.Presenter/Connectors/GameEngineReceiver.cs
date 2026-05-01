using SevenWonders.Game.Engine;

namespace SevenWonders.Game.Presenter.Connectors
{
    public class GameEngineReceiver : IGameEngineReceiver
    {
        public GameEngineReceiver(ISceneManager sceneManager)
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

        public TextLabel ReceiveTextLabel(string name)
        {
            TextLabel? textLabel = m_sceneManager.GetTextLabelByName(name);
            if (textLabel is null)
            {
                throw new InvalidOperationException($"TextLabel with name {name} does not exist.");
            }
            return textLabel;
        }

        public ICollection<TextLabel> ReceiveTextLabels(string name, int number)
        {
            List<TextLabel> result = new List<TextLabel>();
            for (int i = 1; i <= number; i++)
            {
                string targetName = $"{name}{i}";
                TextLabel? textLabel = m_sceneManager.GetTextLabelByName(targetName);
                if (textLabel is null)
                {
                    throw new InvalidOperationException($"TextLabel with name {targetName}, does not exist.");
                }
                result.Add(textLabel);
            }

            return result;
        }

        public IInteractiveObject ReceiveInteractiveObject(string name)
        {
            IInteractiveObject? interactiveObject = m_sceneManager.GetInteractiveObjectByName(name);
            if (interactiveObject is null)
            {
                throw new InvalidOperationException($"GameObject with name {name} does not exist.");
            }
            return interactiveObject;
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

        public GraphicsLayer ReceiveGraphicsLayer(string name)
        {
            GraphicsLayer? layer = m_sceneManager.GetLayerByName(name);
            if (layer is null)
            {
                throw new InvalidOperationException($"GraphicsLayer with name {name} does not exist.");
            }
            return layer;
        }

        public ButtonObject ReceiveButton(string name)
        {
            ButtonObject? button = m_sceneManager.GetButtonByName(name);
            if (button is null)
            {
                throw new InvalidOperationException($"ButtonObject with name {name} does not exist.");
            }
            return button;
        }

        private readonly ISceneManager m_sceneManager;
    }
}
