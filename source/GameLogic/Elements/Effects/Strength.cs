using GameLogic.Events;

namespace GameLogic.Elements.Effects
{
    public class Strength : Effect
    {
        public int Points { get; set; }

        public Strength() { }
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
            gameContext.EventManager.Publish(GameEventType.MilitaryAdvanced, new OnMilitaryAdvanced(gameContext.TurnHandler.CurrentPlayer, Points));
        }
    }
}
