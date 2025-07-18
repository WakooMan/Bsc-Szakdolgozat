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

        public override void Apply()
        {
            throw new NotImplementedException();
        }

        public override Teology Clone()
        {
            return new Teology(this);
        }
    }
}
