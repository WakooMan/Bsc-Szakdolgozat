using GameLogic.Elements;
using GameLogic.Events.GameEvents;

namespace GameLogic.PlayerActions
{
    public class BuildWonderProcess: IPlayerAction
    {
        public string Name => nameof(BuildWonderProcess);
        public int Id => 4;

        public BuildWonderProcess(Player player, ICollection<BuildWonder> buildWonderActions)
        {
            m_backAction = new BasicPlayerAction("BackToTurnDecisions", false);
            m_player = player;
            m_buildWonderActions = [.. buildWonderActions];
        }

        public bool DoPlayerAction(IGameContext gameContext)
        {
            gameContext.EventManager.Publish(new OnBuildWonderProcessStart(m_buildWonderActions, m_backAction));
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
