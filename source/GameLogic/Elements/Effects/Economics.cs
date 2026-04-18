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

        public override Task Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            opponent.OnCardBuilt += OnCardBuilt;
            return Task.CompletedTask;
        }

        public override Task Unapply(IGameContext gameContext, Player owner, Player opponent)
        {
            opponent.OnCardBuilt -= OnCardBuilt;
            return Task.CompletedTask;
        }

        private Task OnCardBuilt(Player owner, OnCardBuilt eventArgs)
        {
            owner.Money += (eventArgs.BuildCost - eventArgs.Card.MoneyCost);
            return Task.CompletedTask;
        }
    }
}
