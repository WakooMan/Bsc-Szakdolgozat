using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Interfaces;

namespace SevenWonders.AI.Model.DecisionRouter
{
    public interface IDecisionRouter
    {
        PlayerActionWrapper RoutePlayerAction(Player player, ICollection<PlayerActionWrapper> playerActions);
    }
}
