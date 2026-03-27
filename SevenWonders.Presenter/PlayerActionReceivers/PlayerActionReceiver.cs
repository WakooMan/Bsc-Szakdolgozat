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
            m_interactiveObjectToPlayerAction = new Dictionary<IInteractiveObject, IPlayerAction>();
        }
        public IPlayerAction ReceivePlayerAction(Player player, ICollection<IPlayerAction> playerActions)
        {
            m_chosenInteractiveObject = null;
            m_signal.Reset();
            m_interactiveObjectToPlayerAction.Clear();
            foreach (IPlayerAction playerAction in playerActions)
            {
                IInteractiveObject interactiveObject = m_gameEngineReceiver.ReceiveInteractiveObject(playerAction.Name);
                m_interactiveObjectToPlayerAction[interactiveObject] = playerAction;
                interactiveObject.ClickedEvent += OnInteractiveObjectClicked;
            }

            while (m_chosenInteractiveObject is null)
            {
                m_signal.Wait();

                if (m_chosenInteractiveObject is not null)
                {
                    foreach (IInteractiveObject interactiveObject in m_interactiveObjectToPlayerAction.Keys)
                    {
                        interactiveObject.ClickedEvent -= OnInteractiveObjectClicked;
                    }
                    return m_interactiveObjectToPlayerAction[m_chosenInteractiveObject];
                }
            }

            throw new InvalidOperationException($"No matching playeraction.");
        }

        private void OnInteractiveObjectClicked(IInteractiveObject interactiveObject, SKTouchEventArgs args)
        {
            m_chosenInteractiveObject = interactiveObject;
            m_signal.Set();
        }

        public void Dispose()
        {
            m_signal?.Dispose();
        }

        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly ManualResetEventSlim m_signal;
        private readonly Dictionary<IInteractiveObject, IPlayerAction> m_interactiveObjectToPlayerAction;
        private IInteractiveObject? m_chosenInteractiveObject;

    }
}
