using GameLogic.Elements;
using GameLogic.Handlers;

namespace GameLogic.Events.GameEvents
{
    public class AfterBuildableBuilt: GameEvent
    {
        public IBuildable Buildable { get; set; }
        public Player Builder { get; set; }
        public Player Opponent { get; set; }

        public AfterBuildableBuilt(Player builder, Player opponent, IBuildable buildable)
        {
            Buildable = buildable;
            Builder = builder;
            Opponent = opponent;
        }
    }
}
