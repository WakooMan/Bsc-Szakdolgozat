using GameLogic;
using GameLogic.Elements;
using GameLogic.Interfaces;
using SevenWonders.AI.Model;

namespace SevenWonders.AITrainerServer.DecisionHandlers
{
    public class CitizenHeuristicBotDecisionHandler : HeuristicBotDecisionHandler, ICitizenHeuristicBotDecisionHandler
    {
        public CitizenHeuristicBotDecisionHandler(IGame game, IWeightConfiguration weightConfiguration) : base(game, weightConfiguration)
        { }

        public PlayerActionWrapper HandleDecisions(Player player, ICollection<PlayerActionWrapper> playerActions)
        {
            return HandlePlayerActions(player, playerActions);
        }
    }
}
