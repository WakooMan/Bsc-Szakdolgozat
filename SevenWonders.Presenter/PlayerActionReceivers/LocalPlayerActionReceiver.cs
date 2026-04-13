using GameLogic.Elements;
using GameLogic.Interfaces;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;
using SevenWonders.WebClient.Model;
using SevenWonders.WebClient.Model.Services;
using SkiaSharp.Views.Maui;
using WebServer.Contract.Messages.Game.ClientMessages;
using WebServer.Contract.Messages.Game.ServerMessages;

namespace SevenWonders.Presenter.PlayerActionReceivers
{
    public class LocalPlayerActionReceiver : ILocalPlayerActionReceiver, IDisposable
    {

        public IClientHubService? ClientHubService { get; set; }

        public LocalPlayerActionReceiver(IGameEngineReceiver gameEngineReceiver)
        {
            m_gameEngineReceiver = gameEngineReceiver;
            m_signal = new ManualResetEventSlim(false);
            m_interactiveObjectToPlayerAction = new Dictionary<IInteractiveObject, PlayerActionWrapper>();
            m_playerActionResponseMessageHandler = new GameResponseMessageHandlerDelegate<PlayerActionResponseMessage>(HandlePlayerActionResponseMessage);
        }

        public PlayerActionWrapper ReceivePlayerAction(Player player, ICollection<PlayerActionWrapper> playerActions)
        {
            m_chosenInteractiveObject = null;
            m_signal.Reset();
            m_interactiveObjectToPlayerAction.Clear();
            foreach (PlayerActionWrapper playerActionWrapper in playerActions)
            {
                IInteractiveObject interactiveObject = m_gameEngineReceiver.ReceiveInteractiveObject(playerActionWrapper.PlayerAction.Name);
                m_interactiveObjectToPlayerAction[interactiveObject] = playerActionWrapper;
                if (playerActionWrapper.CanPerform)
                {
                    interactiveObject.ClickedEvent += OnInteractiveObjectClicked;
                }
                interactiveObject.Dimmed = !playerActionWrapper.CanPerform;
            }

            while (m_chosenInteractiveObject is null)
            {
                m_signal.Wait();

                if (m_chosenInteractiveObject is not null)
                {
                    foreach (IInteractiveObject interactiveObject in m_interactiveObjectToPlayerAction.Keys)
                    {
                        if (!interactiveObject.Dimmed)
                        {
                            interactiveObject.ClickedEvent -= OnInteractiveObjectClicked;
                        }
                        interactiveObject.Dimmed = false;
                    }
                    return m_interactiveObjectToPlayerAction[m_chosenInteractiveObject];
                }
            }

            throw new InvalidOperationException($"No matching playeraction.");
        }

        public void Dispose()
        {
            m_signal?.Dispose();
        }

        public void Register(IMessageRegisterer registerer)
        {
            registerer.Register(m_playerActionResponseMessageHandler);
        }

        public void Unregister(IMessageRegisterer registerer)
        {
            registerer.Unregister(m_playerActionResponseMessageHandler);
        }

        private async void OnInteractiveObjectClicked(IInteractiveObject interactiveObject, SKTouchEventArgs args)
        {
            if (ClientHubService is not null)
            {
                int index = m_interactiveObjectToPlayerAction.Keys.ToList().IndexOf(interactiveObject);
                if (index >= 0)
                {
                    await ClientHubService.InvokeGameCommand(new PlayerActionRequestMessage(index));
                }
            }
            else
            {
                m_chosenInteractiveObject = interactiveObject;
                m_signal.Set();
            }
        }

        private Task<bool> HandlePlayerActionResponseMessage(PlayerActionResponseMessage message)
        {
            var list = m_interactiveObjectToPlayerAction.Keys.ToList();
            if(message.ActionId >= 0 && message.ActionId < list.Count)
            {
                IInteractiveObject interactiveObject = list[message.ActionId];
                m_chosenInteractiveObject = interactiveObject;
                m_signal.Set();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly ManualResetEventSlim m_signal;
        private readonly Dictionary<IInteractiveObject, PlayerActionWrapper> m_interactiveObjectToPlayerAction;
        private readonly GameResponseMessageHandlerDelegate<PlayerActionResponseMessage> m_playerActionResponseMessageHandler;
        private IInteractiveObject? m_chosenInteractiveObject;
    }
}
