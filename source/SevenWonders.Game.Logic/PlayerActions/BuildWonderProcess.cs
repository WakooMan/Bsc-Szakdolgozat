using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Wonders;
using SevenWonders.Game.Logic.Events.GameEvents;

namespace SevenWonders.Game.Logic.PlayerActions
{
    public class BuildWonderProcess: IPlayerAction
    {
        public string Name => nameof(BuildWonderProcess);
        public int Id => 22;

        public BuildWonderProcess(Player player, ICollection<BuildWonder> buildWonderActions)
        {
            m_backAction = new BasicPlayerAction("BackToTurnDecisions", false);
            m_player = player;
            m_buildWonderActions = [.. buildWonderActions];
        }

        public bool DoPlayerAction(IGameContext gameContext)
        {
            Player opponent = gameContext.TurnHandler.OpponentPlayer;
            Dictionary<Wonder, int> costs = m_buildWonderActions.ToDictionary(
                action => action.Wonder,
                action => gameContext.CostCalculator.GetBuildCost(action.Wonder, m_player, opponent));
            gameContext.EventManager.Publish(new OnBuildWonderProcessStart(m_buildWonderActions, m_backAction, costs));
            var (result, playerAction) = gameContext.PlayerActionHandler.HandlePlayerActions(gameContext, m_player, [m_backAction, .. m_buildWonderActions]);
            gameContext.EventManager.Publish(new OnBuildWonderProcessEnd(m_buildWonderActions, m_backAction, result));
            return result;
        }

        public bool CanPerform(IGameContext gameContext)
        {
            var results = m_buildWonderActions.Select(action => action.CanPerform(gameContext));
            return results.Any(result => result);
        }

        private readonly List<BuildWonder> m_buildWonderActions;
        private readonly BasicPlayerAction m_backAction;
        private readonly Player m_player;
    }
}
