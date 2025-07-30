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

        public override void Apply(IGameContext gameContext)
        {
            Player player = gameContext.TurnHandler.CurrentPlayer;
            gameContext.EventManager.Subscribe<OnCardBuilt>(GameEventType.CardBuilt, (args) => OnRedCardBuilt(player, args));
        }

        private PlusStrengthOnRedCardBuild(PlusStrengthOnRedCardBuild plusStrengthOnRedCardBuild)
        {
            AdditionalStrength = plusStrengthOnRedCardBuild.AdditionalStrength.Clone();
        }

        private void OnRedCardBuilt(Player player, OnCardBuilt eventArgs)
        {
            if (eventArgs.Builder == player && eventArgs.Card is RedCard redCard)
            {
                redCard.Strength.Points += AdditionalStrength.Points;
            }
        }
    }
}
