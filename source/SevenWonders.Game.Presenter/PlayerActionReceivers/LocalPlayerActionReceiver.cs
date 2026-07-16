using SevenWonders.Game.Engine.InputHandling;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Exceptions;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Game.Presenter.Connectors;
using SevenWonders.Web.Client.Model;
using SevenWonders.Web.Client.Model.Services;
using SevenWonders.Web.Server.Contract.Messages.Game.ClientMessages;
using SevenWonders.Web.Server.Contract.Messages.Game.ServerMessages;
using SkiaSharp.Views.Maui;

namespace SevenWonders.Game.Presenter.PlayerActionReceivers
{
    public class LocalPlayerActionReceiver : ILocalPlayerActionReceiver, IDisposable
    {

        public IClientHubService? ClientHubService { get; set; }

        public LocalPlayerActionReceiver(IGameEngineReceiver gameEngineReceiver, string playerName, IClientMessageDispatcher clientMessageDispatcher)
        {
            m_playerName = playerName;
            m_gameEngineReceiver = gameEngineReceiver;
            m_signal = new ManualResetEventSlim(false);
            m_interactiveObjectToPlayerAction = new Dictionary<IInteractiveObject, PlayerActionWrapper>();
            m_clientMessageDispatcher = clientMessageDispatcher;
            m_playerActionResponseMessageHandler = new GameResponseMessageHandlerDelegate<PlayerActionResponseMessage>(HandlePlayerActionResponseMessage);
            m_clientMessageDispatcher.RegisterHandler(this);
            m_isEnded = false;
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

            while (m_chosenInteractiveObject is null && !m_isEnded)
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

            if (m_isEnded)
            {
                throw new EndGameException();
            }

            throw new InvalidOperationException($"No matching playeraction.");
        }

        public void Dispose()
        {
            m_clientMessageDispatcher?.UnregisterHandler(this);
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
                    await ClientHubService.InvokeGameCommand(new PlayerActionRequestMessage(m_playerName, index, m_interactiveObjectToPlayerAction.Values.Select(key => key.PlayerAction.Id).ToList()));
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
            if (message.Success && message.PlayerName == m_playerName)
            {
                var list = m_interactiveObjectToPlayerAction.Keys.ToList();
                if (message.ActionId >= 0 && message.ActionId < list.Count)
                {
                    IInteractiveObject interactiveObject = list[message.ActionId];
                    m_chosenInteractiveObject = interactiveObject;
                    m_signal.Set();
                }
            }
            return Task.FromResult(message.Success);
        }

        public void EndGame()
        {
            m_isEnded = true;
            m_chosenInteractiveObject = null;
            m_signal.Set();
        }

        private readonly string m_playerName;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly ManualResetEventSlim m_signal;
        private readonly Dictionary<IInteractiveObject, PlayerActionWrapper> m_interactiveObjectToPlayerAction;
        private readonly GameResponseMessageHandlerDelegate<PlayerActionResponseMessage> m_playerActionResponseMessageHandler;
        private readonly IClientMessageDispatcher m_clientMessageDispatcher;
        private IInteractiveObject? m_chosenInteractiveObject;
        private bool m_isEnded;
    }
}
