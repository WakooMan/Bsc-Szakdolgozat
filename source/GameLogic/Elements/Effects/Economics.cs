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

        public override void Apply(IGameContext gameContext)
        {
            Player player = gameContext.TurnHandler.CurrentPlayer;
            gameContext.EventManager.Subscribe<OnCardBuilt>((args) => OnCardBuilt(player, args));
        }

        private static void OnCardBuilt(Player player, OnCardBuilt eventArgs)
        {
            if (player != eventArgs.Builder)
            {
                player.Money += eventArgs.BuildCost;
            }
        }
    }
}
