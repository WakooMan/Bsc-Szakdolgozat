using GameLogic;
using GameLogic.Elements;
using GameLogic.Interfaces;
using SevenWonders.AI.Model;

namespace SevenWonders.AITrainerServer.DecisionHandlers
{
    public class ScientificHeuristicBotDecisionHandler : HeuristicBotDecisionHandler, IScientificHeuristicBotDecisionHandler
    {
        public ScientificHeuristicBotDecisionHandler(IGame game, IWeightConfiguration weightConfiguration) : base(game, weightConfiguration)
        {
            m_disciplineScore = 10.0f;
            m_cumulativeDisciplineScore = 20.0f;
            m_chooseDevelopmentScore = 15f;
        }

        public PlayerActionWrapper HandleDecisions(Player player, ICollection<PlayerActionWrapper> playerActions)
        {
            return HandlePlayerActions(player, playerActions);
        }

    }
}
