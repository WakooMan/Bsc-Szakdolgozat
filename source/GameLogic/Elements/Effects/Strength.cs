using GameLogic.Events;
using GameLogic.Events.GameEvents;

namespace GameLogic.Elements.Effects
{
    public class Strength : Effect
    {
        public int Points { get; set; }

        public Strength()
        {
            Points = 0;
        }
        private Strength(Strength strength)
        {
            Points = strength.Points;
        }

        public override Strength Clone()
        {
            return new Strength(this);
        }

        public override void Apply(IGameContext gameContext)
        {
            gameContext.EventManager.Publish(new OnMilitaryAdvanced(gameContext.TurnHandler.CurrentPlayer, Points));
        }
    }
}
