using SevenWonders.Game.Engine.InputHandling;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Exceptions;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Game.Presenter.Connectors;
using SevenWonders.Web.Client.Model;
using SevenWonders.Web.Server.Contract.Messages.Game.ServerMessages;

namespace SevenWonders.Game.Presenter.PlayerActionReceivers
{
    public class RemotePlayerActionReceiver: IRemotePlayerActionReceiver
    {
        public RemotePlayerActionReceiver(IGameEngineReceiver gameEngineReceiver, string playerName, IClientMessageDispatcher clientMessageDispatcher)
        {
            m_gameEngineReceiver = gameEngineReceiver;
            m_signal = new ManualResetEventSlim(false);
            m_playerActionToInteractiveObject = new Dictionary<PlayerActionWrapper, IInteractiveObject>();
            m_serverPlayerActionMessageHandler = new GameResponseMessageHandlerDelegate<ServerPlayerActionMessage>(HandleServerPlayerActionMessage);
            m_clientMessageDispatcher = clientMessageDispatcher;
            m_playerName = playerName;
            m_clientMessageDispatcher.RegisterHandler(this);
            m_isEnded = false;
        }

        public PlayerActionWrapper ReceivePlayerAction(Player player, ICollection<PlayerActionWrapper> playerActions)
        {
            m_chosenPlayerAction = null;
            m_signal.Reset();
            m_playerActionToInteractiveObject.Clear();
            foreach (PlayerActionWrapper playerActionWrapper in playerActions)
            {
                IInteractiveObject interactiveObject = m_gameEngineReceiver.ReceiveInteractiveObject(playerActionWrapper.PlayerAction.Name);
                m_playerActionToInteractiveObject[playerActionWrapper] = interactiveObject;
                interactiveObject.Dimmed = true;
            }

            while (m_chosenPlayerAction is null && !m_isEnded)
            {
                m_signal.Wait();

                if (m_chosenPlayerAction is not null)
                {
                    foreach (IInteractiveObject interactiveObject in m_playerActionToInteractiveObject.Values)
                    {
                        interactiveObject.Dimmed = false;
                    }
                    return m_chosenPlayerAction;
                }
            }

            if (m_isEnded)
            {
                throw new EndGameException();
            }

            throw new InvalidOperationException($"No matching playeraction.");
        }

        private Task<bool> HandleServerPlayerActionMessage(ServerPlayerActionMessage message)
        {
            if (message.Success && message.PlayerName == m_playerName)
            {
                var actions = m_playerActionToInteractiveObject.Keys.ToArray();
                if (message.ActionId >= 0 && message.ActionId < actions.Length)
                {
                    m_chosenPlayerAction = actions[message.ActionId];
                    m_signal.Set();
                }
            }
            return Task.FromResult(message.Success);
        }

        public void Dispose()
        {
            m_clientMessageDispatcher?.UnregisterHandler(this);
            m_signal?.Dispose();
        }

        public void Register(IMessageRegisterer registerer)
        {
            registerer.Register(m_serverPlayerActionMessageHandler);
        }

        public void Unregister(IMessageRegisterer registerer)
        {
            registerer.Unregister(m_serverPlayerActionMessageHandler);
        }

        public void EndGame()
        {
            m_isEnded = true;
            m_chosenPlayerAction = null;
            m_signal.Set();
        }

        private readonly string m_playerName;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly ManualResetEventSlim m_signal;
        private readonly Dictionary<PlayerActionWrapper, IInteractiveObject> m_playerActionToInteractiveObject;
        private readonly GameResponseMessageHandlerDelegate<ServerPlayerActionMessage> m_serverPlayerActionMessageHandler;
        private readonly IClientMessageDispatcher m_clientMessageDispatcher;
        private PlayerActionWrapper? m_chosenPlayerAction;
        private bool m_isEnded;

    }
}
