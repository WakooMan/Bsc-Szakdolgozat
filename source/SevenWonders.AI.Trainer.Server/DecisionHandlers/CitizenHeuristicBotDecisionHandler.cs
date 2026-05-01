using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.AI.Model;

namespace SevenWonders.AI.Trainer.Server.DecisionHandlers
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
