using GameLogic.Interfaces;
using SevenWonders.AI.Model.Cache;
using SevenWonders.AI.Model.DecisionRouter.DecisionHandlers;
using SevenWonders.AI.Model.DecisionRouter.Factories;
using SevenWonders.AI.Model.PlayerActionReceivers;
using SevenWonders.Common;
using SevenWonders.Presenter.Connectors;
using SevenWonders.WebClient.Model;
using SevenWonders.WebClient.Model.Services;

namespace SevenWonders.Presenter.PlayerActionReceivers
{
    public class PlayerActionReceiverFactory : IPlayerActionReceiverFactory
    {
        public PlayerActionReceiverFactory(IGameEngineReceiver gameEngineReceiver, 
                                           IClientHubService clientHubService, 
                                           IClientMessageDispatcher clientMessageDispatcher,
                                           IDecisionRouterFactory decisionRouterFactory,
                                           IAIDecisionHandlerCache aIDecisionHandlerCache)
        {
            m_gameEngineReceiver = gameEngineReceiver;
            m_clientHubService = clientHubService;
            m_clientMessageDispatcher = clientMessageDispatcher;
            m_decisionRouterFactory = decisionRouterFactory;
            m_aIDecisionHandlerCache = aIDecisionHandlerCache;
        }

        public IPlayerActionReceiver Create(PlayerType playerType, string playerName)
        {
            switch (playerType)
            {
                case PlayerType.LocalPlayer:
                    return new LocalPlayerActionReceiver(m_gameEngineReceiver, playerName, m_clientMessageDispatcher);
                case PlayerType.LocalPlayerWithRemoteOpponent:
                    var result = new LocalPlayerActionReceiver(m_gameEngineReceiver, playerName, m_clientMessageDispatcher);
                    result.ClientHubService = m_clientHubService;
                    return result;
                case PlayerType.RemotePlayer:
                    return new RemotePlayerActionReceiver(m_gameEngineReceiver, playerName, m_clientMessageDispatcher);
                case PlayerType.EasyAI:
                    return new NonPlayerActionReceiver(m_decisionRouterFactory, m_aIDecisionHandlerCache.EasyAI);
                case PlayerType.MediumAI:
                    return new NonPlayerActionReceiver(m_decisionRouterFactory, m_aIDecisionHandlerCache.MediumAI);
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerType), playerType, "Not handled");
            }
        }

        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IClientHubService m_clientHubService;
        private readonly IClientMessageDispatcher m_clientMessageDispatcher;
        private readonly IDecisionRouterFactory m_decisionRouterFactory;
        private readonly IAIDecisionHandlerCache m_aIDecisionHandlerCache;
    }
}
