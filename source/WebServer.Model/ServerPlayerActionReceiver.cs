using GameLogic.Elements;
using GameLogic.Interfaces;
using Microsoft.AspNetCore.SignalR;
using WebServer.Contract.Messages.Game.ClientMessages;
using WebServer.Contract.Messages.Game.ServerMessages;
using WebServer.Model.Client;
using WebServer.Model.MessageHandling;

namespace WebServer.Model
{
    public class ServerPlayerActionReceiver: IPlayerActionReceiver, IMessageHandler
    {
        public ServerPlayerActionReceiver(IPlayerClient player)
        {
            m_player = player;
            m_signal = new ManualResetEventSlim(false);
            m_playerActions = new List<PlayerActionWrapper>();
            m_playerActionRequestMessageHandler = new GameRequestMessageHandlerDelegate<PlayerActionRequestMessage>(HandlePlayerActionRequestMessage);
        }

        public PlayerActionWrapper ReceivePlayerAction(Player player, ICollection<PlayerActionWrapper> playerActions)
        {
            m_chosenPlayerAction = null;
            m_signal.Reset();
            m_playerActions.Clear();
            m_playerActions.AddRange(playerActions);

            while (m_chosenPlayerAction is null)
            {
                m_signal.Wait();

                if (m_chosenPlayerAction is not null)
                {
                    return m_chosenPlayerAction;
                }
            }

            throw new InvalidOperationException($"No matching playeraction.");
        }

        private Task<GameServerMessage> HandlePlayerActionRequestMessage(Hub hub, string connectionId, PlayerActionRequestMessage message)
        {
            if (m_player.ConnectionId != connectionId)
            {
                return Task.FromResult<GameServerMessage>(new PlayerActionResponseMessage("Wrong player sent the action!"));
            }

            if (message.ActionId >= 0 && message.ActionId < m_playerActions.Count)
            {
                m_chosenPlayerAction = m_playerActions[message.ActionId];
                m_signal.Set();
                return Task.FromResult<GameServerMessage>(new PlayerActionResponseMessage(m_player.ApplicationUser.UserName, message.ActionId));
            }
            return Task.FromResult<GameServerMessage>(new PlayerActionResponseMessage("The received action id is not a valid player action!"));
        }

        public void Dispose()
        {
            m_signal?.Dispose();
        }

        public void Register(IMessageRegisterer registerer)
        {
            registerer.Register(m_playerActionRequestMessageHandler);
        }

        public void Unregister(IMessageRegisterer registerer)
        {
            registerer.Unregister(m_playerActionRequestMessageHandler);
        }

        private readonly ManualResetEventSlim m_signal;
        private readonly List<PlayerActionWrapper> m_playerActions;
        private readonly GameRequestMessageHandlerDelegate<PlayerActionRequestMessage> m_playerActionRequestMessageHandler;
        private readonly IPlayerClient m_player;
        private PlayerActionWrapper? m_chosenPlayerAction;
    }
}
