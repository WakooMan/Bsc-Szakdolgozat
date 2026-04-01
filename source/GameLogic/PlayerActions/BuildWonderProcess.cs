using GameLogic.Elements;
using GameLogic.Events.GameEvents;

namespace GameLogic.PlayerActions
{
    public class BuildWonderProcess: IPlayerAction
    {
        public string Name => nameof(BuildWonderProcess);

        public BuildWonderProcess(Player player, ICollection<BuildWonder> buildWonderActions)
        {
            m_backAction = new BasicPlayerAction("BackToTurnDecisions", false);
            m_player = player;
            m_buildWonderActions = [.. buildWonderActions];
        }

        public async Task<bool> DoPlayerAction(IGameContext gameContext)
        {
            await gameContext.EventManager.PublishAsync(new OnBuildWonderProcessStart(m_buildWonderActions, m_backAction));
            bool result = await gameContext.PlayerActionHandler.HandlePlayerActions(gameContext, m_player, [m_backAction, .. m_buildWonderActions]);
            await gameContext.EventManager.PublishAsync(new OnBuildWonderProcessEnd(m_buildWonderActions, m_backAction, result));
            return result;
        }

        public async Task<bool> CanPerform(IGameContext gameContext)
        {
            var results = await Task.WhenAll(m_buildWonderActions.Select(action => action.CanPerform(gameContext)));
            return results.Any(result => result);
        }

        private readonly List<BuildWonder> m_buildWonderActions;
        private readonly BasicPlayerAction m_backAction;
        private readonly Player m_player;
    }
}
