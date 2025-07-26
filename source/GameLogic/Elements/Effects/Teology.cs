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

        public override void Apply(Player player, IEventManager eventManager)
        {
            eventManager.Subscribe(GameEventType.WonderBuilt, (args) => OnWonderBuilt(player, args));
        }

        private void OnWonderBuilt(Player player, EventArgs args)
        {
            if (args is OnWonderBuilt wonderBuilt && wonderBuilt.Builder == player && !wonderBuilt.Wonder.Effects.Any(effect => effect is NewTurn))
            {
                wonderBuilt.Wonder.Effects.Add(new NewTurn());
            }
        }
    }
}
