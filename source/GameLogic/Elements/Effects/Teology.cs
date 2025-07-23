using GameLogic.GameStates;

namespace GameLogic.Elements.Effects
{
    public class Teology : Effect
    {
        public NewTurn NewTurn { get; set; }

        public Teology() { }

        private Teology(Teology teology)
        {
            NewTurn = teology.NewTurn.Clone();
        }

        public override void Apply(PlayingState game)
        {
            throw new NotImplementedException();
        }

        public override Teology Clone()
        {
            return new Teology(this);
        }
    }
}
