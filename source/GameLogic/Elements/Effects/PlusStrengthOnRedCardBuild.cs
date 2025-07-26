using GameLogic.Elements.GameCards;
using GameLogic.Events;

namespace GameLogic.Elements.Effects
{
    public class PlusStrengthOnRedCardBuild : Effect
    {
        public Strength AdditionalStrength { get; set; }

        public PlusStrengthOnRedCardBuild() { }

        public override PlusStrengthOnRedCardBuild Clone()
        {
            return new PlusStrengthOnRedCardBuild(this);
        }

        public override void Apply(Player player, IEventManager eventManager)
        {
            eventManager.Subscribe(GameEventType.CardBuilt, (args) => OnRedCardBuilt(player, args));
        }

        private PlusStrengthOnRedCardBuild(PlusStrengthOnRedCardBuild plusStrengthOnRedCardBuild)
        {
            AdditionalStrength = plusStrengthOnRedCardBuild.AdditionalStrength.Clone();
        }

        private void OnRedCardBuilt(Player player, EventArgs args)
        {
            
            if (args is OnCardBuilt eventArgs && eventArgs.Builder == player && eventArgs.Card is RedCard redCard)
            {
                redCard.Strength.Points += 1;
            }
        }
    }
}
