using GameLogic.Events.GameEvents;

namespace GameLogic.Elements.Effects
{
    public class Economics : Effect
    {
        public Economics() { }
        

        public override Economics Clone()
        {
            return new Economics();
        }

        public override void Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            opponent.OnCardBuilt += OnCardBuilt;
        }

        public override void Unapply(IGameContext gameContext, Player owner, Player opponent)
        {
            opponent.OnCardBuilt -= OnCardBuilt;
        }

        private Task OnCardBuilt(Player owner, OnCardBuilt eventArgs)
        {
            owner.Money += (eventArgs.BuildCost - eventArgs.Card.MoneyCost);
            return Task.CompletedTask;
        }
    }
}
