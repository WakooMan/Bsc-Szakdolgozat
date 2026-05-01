using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.AI.Model;

namespace SevenWonders.AI.Trainer.Server.DecisionHandlers
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
