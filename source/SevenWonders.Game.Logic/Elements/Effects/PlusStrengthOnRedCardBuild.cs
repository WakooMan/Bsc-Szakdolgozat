using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events.GameEvents;

namespace SevenWonders.Game.Logic.Elements.Effects
{
    public class PlusStrengthOnRedCardBuild : Effect
    {
        public Strength AdditionalStrength { get; set; }

        public PlusStrengthOnRedCardBuild()
        {
            AdditionalStrength = new Strength();
        }

        public override PlusStrengthOnRedCardBuild Clone()
        {
            return new PlusStrengthOnRedCardBuild(this);
        }

        public override void Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.OnCardBuilt += OnRedCardBuilt;
        }

        public override void Unapply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.OnCardBuilt -= OnRedCardBuilt;
        }

        private PlusStrengthOnRedCardBuild(PlusStrengthOnRedCardBuild plusStrengthOnRedCardBuild)
        {
            AdditionalStrength = plusStrengthOnRedCardBuild.AdditionalStrength.Clone();
        }

        private Task OnRedCardBuilt(Player owner, OnCardBuilt eventArgs)
        {
            if (eventArgs.Card is RedCard redCard)
            {
                redCard.Strength.Points += AdditionalStrength.Points;
            }
            return Task.CompletedTask;
        }
    }
}
