using GameLogic.Elements;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;
using SkiaSharp.Views.Maui;

namespace SevenWonders.Presenter.PlayerActionReceivers
{
    public class PlayerActionReceiver : IPlayerActionReceiver, IDisposable
    {
        public PlayerActionReceiver(IGameEngineReceiver gameEngineReceiver)
        {
            m_gameEngineReceiver = gameEngineReceiver;
            m_signal = new ManualResetEventSlim(false);
            m_gameObjectToPlayerAction = new Dictionary<GameObject, IPlayerAction>();
        }
        public IPlayerAction ReceivePlayerAction(Player player, ICollection<IPlayerAction> playerActions)
        {
            m_chosenGameObject = null;
            m_signal.Reset();
            m_gameObjectToPlayerAction.Clear();
            foreach (IPlayerAction playerAction in playerActions)
            {
                GameObject gameObject = m_gameEngineReceiver.ReceiveGameObject(playerAction.Name);
                m_gameObjectToPlayerAction[gameObject] = playerAction;
                gameObject.ClickedEvent += OnGameObjectClicked;
            }

            while (m_chosenGameObject is null)
            {
                m_signal.Wait();

                if (m_chosenGameObject is not null)
                {
                    foreach (GameObject gameObject in m_gameObjectToPlayerAction.Keys)
                    {
                        gameObject.ClickedEvent -= OnGameObjectClicked;
                    }
                    return m_gameObjectToPlayerAction[m_chosenGameObject];
                }
            }

            throw new InvalidOperationException($"No matching playeraction.");
        }

        private void OnGameObjectClicked(GameObject gameObject, SKTouchEventArgs args)
        {
            m_chosenGameObject = gameObject;
            m_signal.Set();
        }

        public void Dispose()
        {
            m_signal?.Dispose();
        }

        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly ManualResetEventSlim m_signal;
        private readonly Dictionary<GameObject, IPlayerAction> m_gameObjectToPlayerAction;
        private GameObject? m_chosenGameObject;

    }
}
