using GameLogic.Interfaces;
using SevenWonders.Common;
using SevenWonders.Presenter.Connectors;
using SevenWonders.WebClient.Model.Services;
using WebServer.Contract.Messages.Game.ServerMessages;

namespace SevenWonders.Presenter.PlayerActionReceivers
{
    public class PlayerActionReceiverFactory : IPlayerActionReceiverFactory
    {
        public PlayerActionReceiverFactory(IGameEngineReceiver gameEngineReceiver, IClientHubService clientHubService)
        {
            m_gameEngineReceiver = gameEngineReceiver;
            m_clientHubService = clientHubService;
        }

        public IPlayerActionReceiver Create(PlayerType playerType, string playerName)
        {
            switch (playerType)
            {
                case PlayerType.LocalPlayer:
                    return new LocalPlayerActionReceiver(m_gameEngineReceiver, playerName);
                case PlayerType.LocalPlayerWithRemoteOpponent:
                    var result = new LocalPlayerActionReceiver(m_gameEngineReceiver, playerName);
                    result.ClientHubService = m_clientHubService;
                    return result;
                case PlayerType.RemotePlayer:
                    return new RemotePlayerActionReceiver(m_gameEngineReceiver, playerName);
                case PlayerType.AI:
                    throw new NotSupportedException("AI feature is not yet supported!");
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerType), playerType, "Not handled");
            }
        }

        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IClientHubService m_clientHubService;
    }
}
