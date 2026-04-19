using GameLogic.Elements;
using GameLogic.Interfaces;

namespace SevenWonders.AI.Model.DecisionRouter
{
    public interface IDecisionRouter
    {
        PlayerActionWrapper RoutePlayerAction(Player player, ICollection<PlayerActionWrapper> playerActions);
    }
}
