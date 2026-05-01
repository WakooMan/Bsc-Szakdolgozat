using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.AI.Model;

namespace SevenWonders.AI.Trainer.Server.DecisionHandlers
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
