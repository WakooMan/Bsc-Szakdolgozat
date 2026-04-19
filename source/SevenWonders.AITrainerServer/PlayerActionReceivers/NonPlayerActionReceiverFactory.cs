using GameLogic.Interfaces;
using SevenWonders.AI.Model.DecisionRouter.DecisionHandlers;
using SevenWonders.AI.Model.DecisionRouter.Factories;
using SevenWonders.AI.Model.PlayerActionReceivers;
using SevenWonders.AITrainerServer.DecisionHandlers;

namespace SevenWonders.AITrainerServer.PlayerActionReceivers
{
    public class NonPlayerActionReceiverFactory : INonPlayerActionReceiverFactory
    {
        public NonPlayerActionReceiverFactory(IDecisionRouterFactory decisionRouterFactory,
                                              IAIDecisionHandler aIDecisionHandler, 
                                              IMilitaryHeuristicBotDecisionHandler militaryHeuristicBotDecisionHandler,
                                              IRandomBotDecisionHandler randomBotDecisionHandler,
                                              IScientificHeuristicBotDecisionHandler scientificHeuristicBotDecisionHandler,
                                              ICitizenHeuristicBotDecisionHandler citizenHeuristicBotDecisionHandler)
        {
            m_decisionRouterFactory = decisionRouterFactory;
            m_aIDecisionHandler = aIDecisionHandler;
            m_militaryHeuristicBotDecisionHandler = militaryHeuristicBotDecisionHandler;
            m_randomBotDecisionHandler = randomBotDecisionHandler;
            m_scientificHeuristicBotDecisionHandler = scientificHeuristicBotDecisionHandler;
            m_citizenHeuristicBotDecisionHandler = citizenHeuristicBotDecisionHandler;
        }
        public IPlayerActionReceiver CreateNonPlayerActionReceiver(NonPlayerType nonPlayerType)
        {
            switch(nonPlayerType)
            {
                case NonPlayerType.AI:
                    return new NonPlayerActionReceiver(m_decisionRouterFactory, m_aIDecisionHandler);
                case NonPlayerType.MilitaryHeuristicBot:
                    return new NonPlayerActionReceiver(m_decisionRouterFactory, m_militaryHeuristicBotDecisionHandler);
                case NonPlayerType.RandomBot:
                    return new NonPlayerActionReceiver(m_decisionRouterFactory, m_randomBotDecisionHandler);
                case NonPlayerType.ScientificHeuristicBot:
                    return new NonPlayerActionReceiver(m_decisionRouterFactory, m_scientificHeuristicBotDecisionHandler);
                case NonPlayerType.CitizenHeuristicBot:
                    return new NonPlayerActionReceiver(m_decisionRouterFactory, m_citizenHeuristicBotDecisionHandler);
                default:
                    throw new ArgumentOutOfRangeException(nameof(nonPlayerType), nonPlayerType, null);
            }
        }

        private readonly IAIDecisionHandler m_aIDecisionHandler;
        private readonly IMilitaryHeuristicBotDecisionHandler m_militaryHeuristicBotDecisionHandler;
        private readonly IScientificHeuristicBotDecisionHandler m_scientificHeuristicBotDecisionHandler;
        private readonly ICitizenHeuristicBotDecisionHandler m_citizenHeuristicBotDecisionHandler;
        private readonly IRandomBotDecisionHandler m_randomBotDecisionHandler;
        private readonly IDecisionRouterFactory m_decisionRouterFactory;
    }
}
