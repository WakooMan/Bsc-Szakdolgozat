using GameLogic.Elements;
using GameLogic.Interfaces;

namespace SevenWonders.AI.Model.DecisionRouter
{
    public interface IDecisionHandler
    {
        PlayerActionWrapper HandleDecisions(Player player,  ICollection<PlayerActionWrapper> playerActions);
    }
}
