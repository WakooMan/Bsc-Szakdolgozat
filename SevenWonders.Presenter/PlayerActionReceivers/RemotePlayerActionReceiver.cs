using GameLogic.Elements;
using GameLogic.Interfaces;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;
using SevenWonders.WebClient.Model;
using WebServer.Contract.Messages.Game.ServerMessages;

namespace SevenWonders.Presenter.PlayerActionReceivers
{
    public class RemotePlayerActionReceiver: IRemotePlayerActionReceiver
    {
        public RemotePlayerActionReceiver(IGameEngineReceiver gameEngineReceiver)
        {
            m_gameEngineReceiver = gameEngineReceiver;
            m_signal = new ManualResetEventSlim(false);
            m_playerActionToInteractiveObject = new Dictionary<PlayerActionWrapper, IInteractiveObject>();
            m_serverPlayerActionMessageHandler = new GameResponseMessageHandlerDelegate<ServerPlayerActionMessage>(HandleServerPlayerActionMessage);
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

            while (m_chosenPlayerAction is null)
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

            throw new InvalidOperationException($"No matching playeraction.");
        }

        private Task<bool> HandleServerPlayerActionMessage(ServerPlayerActionMessage message)
        {
            var actions = m_playerActionToInteractiveObject.Keys.ToArray();
            if(message.ActionId >= 0 && message.ActionId < actions.Length)
            {
                m_chosenPlayerAction = actions[message.ActionId];
                m_signal.Set();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public void Dispose()
        {
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

        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly ManualResetEventSlim m_signal;
        private readonly Dictionary<PlayerActionWrapper, IInteractiveObject> m_playerActionToInteractiveObject;
        private readonly GameResponseMessageHandlerDelegate<ServerPlayerActionMessage> m_serverPlayerActionMessageHandler;
        private PlayerActionWrapper? m_chosenPlayerAction;

    }
}
