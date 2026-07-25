using SevenWonders.Game.Logic.Elements.Wonders;
using SevenWonders.Game.Logic.PlayerActions;

namespace SevenWonders.Game.Logic.Events.GameEvents
{
    public class OnBuildWonderProcessStart : GameEvent
    {
        public ICollection<BuildWonder> BuildWonderActions { get; }
        public BasicPlayerAction BackAction { get; }
        public IReadOnlyDictionary<Wonder, int> Costs { get; }

        public OnBuildWonderProcessStart(ICollection<BuildWonder> buildWonderActions, BasicPlayerAction backAction, IReadOnlyDictionary<Wonder, int> costs)
        {
            BuildWonderActions = buildWonderActions;
            BackAction = backAction;
            Costs = costs;
        }
    }
}
