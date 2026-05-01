using SevenWonders.Game.Logic.PlayerActions;

namespace SevenWonders.Game.Logic.Events.GameEvents
{
    public class OnBuildWonderProcessStart : GameEvent
    {
        public ICollection<BuildWonder> BuildWonderActions { get; }
        public BasicPlayerAction BackAction { get; }

        public OnBuildWonderProcessStart(ICollection<BuildWonder> buildWonderActions, BasicPlayerAction backAction)
        {
            BuildWonderActions = buildWonderActions;
            BackAction = backAction;
        }
    }
}
