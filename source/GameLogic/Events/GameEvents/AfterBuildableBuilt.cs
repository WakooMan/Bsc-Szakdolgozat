using GameLogic.Elements;
using GameLogic.Handlers;

namespace GameLogic.Events.GameEvents
{
    public class AfterBuildableBuilt: GameEvent
    {
        public IBuildable Buildable { get; set; }
        public Player Builder { get; set; }

        public AfterBuildableBuilt(Player builder, IBuildable buildable)
        {
            Buildable = buildable;
            Builder = builder;
        }
    }
}
