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

        public override Task Apply(IGameContext gameContext)
        {
            if (!AlreadyApplied)
            {
                gameContext.TurnHandler.ForceNewTurn();
                AlreadyApplied = true;
            }
            return Task.CompletedTask;
        }

       

        private NewTurn(NewTurn newTurn)
        {
            AlreadyApplied = newTurn.AlreadyApplied;
        }

    }
}
