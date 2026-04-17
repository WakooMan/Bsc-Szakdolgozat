using GameLogic.Events.GameEvents;

namespace GameLogic.Elements.Effects
{
    public class Teology : Effect
    {
        public Teology() { }

        public override Teology Clone()
        {
            return new Teology();
        }

        public override Task Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.OnWonderBuilt += OnWonderBuilt;
            return Task.CompletedTask;
        }

        public override Task Unapply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.OnWonderBuilt -= OnWonderBuilt;
            return Task.CompletedTask;
        }

        private Task OnWonderBuilt(Player owner, OnWonderBuilt args)
        {
            if (!args.Wonder.Effects.Any(effect => effect is NewTurn))
            {
                args.Wonder.Effects.Add(new NewTurn());
            }
            return Task.CompletedTask;
        }
    }
}
