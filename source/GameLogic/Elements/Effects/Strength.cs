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

        public override async Task Apply(IGameContext gameContext, int playerId)
        {
            await gameContext.EventManager.PublishAsync(new OnMilitaryAdvanced(gameContext.TurnHandler.CurrentPlayer, Points));
        }

        public override async Task Unapply(IGameContext gameContext, int playerId)
        {
            await gameContext.EventManager.PublishAsync(new OnMilitaryAdvanced(gameContext.TurnHandler.OpponentPlayer, Points));
        }
    }
}
