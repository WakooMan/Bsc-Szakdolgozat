using GameLogic;
using GameLogic.Elements;
using GameLogic.Interfaces;
using SevenWonders.AI.Model;

namespace SevenWonders.AITrainerServer.DecisionHandlers
{
    public class MilitaryHeuristicBotDecisionHandler : HeuristicBotDecisionHandler, IMilitaryHeuristicBotDecisionHandler
    {

        public MilitaryHeuristicBotDecisionHandler(IGame game, IWeightConfiguration weightConfiguration) : base(game, weightConfiguration)
        {
            m_cumulativeStrengthScore = 20.0f;
            m_strengthScore = 6.0f;
        }

        public PlayerActionWrapper HandleDecisions(Player player, ICollection<PlayerActionWrapper> playerActions)
        {
            return HandlePlayerActions(player, playerActions);
        }
    }
}
