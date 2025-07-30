using GameLogic.Events;

namespace GameLogic.Elements.Effects
{
    public class Teology : Effect
    {
        public Teology() { }

        public override Teology Clone()
        {
            return new Teology();
        }

        public override void Apply(IGameContext gameContext)
        {
            Player player = gameContext.TurnHandler.CurrentPlayer;
            gameContext.EventManager.Subscribe<OnWonderBuilt>(GameEventType.WonderBuilt, (args) => OnWonderBuilt(player, args));
        }

        private void OnWonderBuilt(Player player, OnWonderBuilt args)
        {
            if (args.Builder == player && !args.Wonder.Effects.Any(effect => effect is NewTurn))
            {
                args.Wonder.Effects.Add(new NewTurn());
            }
        }
    }
}
