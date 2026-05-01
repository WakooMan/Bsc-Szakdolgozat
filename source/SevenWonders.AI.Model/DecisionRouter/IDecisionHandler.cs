using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Interfaces;

namespace SevenWonders.AI.Model.DecisionRouter
{
    public interface IDecisionHandler
    {
        PlayerActionWrapper HandleDecisions(Player player,  ICollection<PlayerActionWrapper> playerActions);
    }
}
