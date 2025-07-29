using GameLogic.Events;

namespace GameLogic.Elements.Effects
{
    public class NewTurn : Effect
    {
        public bool AlreadyApplied { get; set; }

        public NewTurn()
        {
            AlreadyApplied = false;
        }

        public override NewTurn Clone()
        {
            return new NewTurn(this);
        }

        public override void Apply(IGameContext gameContext)
        {
            Player player = gameContext.TurnHandler.CurrentPlayer;
            gameContext.EventManager.Subscribe(GameEventType.TurnEnded, (args) => OnTurnEnded(player, args));
        }

        private void OnTurnEnded(Player player, EventArgs args)
        {
            if (!AlreadyApplied && args is TurnEnded turnEnded && turnEnded.TurnHandler.CurrentPlayer == player)
            {
                turnEnded.TurnHandler.ForceNewTurn();
                AlreadyApplied = true;
            }
        }

        private NewTurn(NewTurn newTurn)
        {
            AlreadyApplied = newTurn.AlreadyApplied;
        }

    }
}
