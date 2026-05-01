using SevenWonders.Game.Logic.PlayerActions;

namespace SevenWonders.Game.Logic.Events.GameEvents
{
    public class OnBuildWonderProcessEnd: GameEvent
    {
        public ICollection<BuildWonder> BuildWonderActions { get; }
        public BasicPlayerAction BackAction { get; }
        public bool IsCompleted { get; }

        public OnBuildWonderProcessEnd(ICollection<BuildWonder> buildWonderActions, BasicPlayerAction backAction, bool isCompleted)
        {
            BuildWonderActions = buildWonderActions;
            BackAction = backAction;
            IsCompleted = isCompleted;
        }
    }
}
