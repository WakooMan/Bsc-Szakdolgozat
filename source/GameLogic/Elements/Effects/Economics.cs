using GameLogic.Events;

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
            gameContext.EventManager.Subscribe(GameEventType.CardBuilt, (args) => OnCardBuilt(player, args));
        }

        private void OnCardBuilt(Player player, EventArgs args)
        {
            if (args is OnCardBuilt eventArgs && player != eventArgs.Builder)
            {
                player.Money += eventArgs.BuildCost;
            }
        }
    }
}
