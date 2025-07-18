using GameLogic.Elements.Disciplines;

namespace GameLogic.Elements.Effects
{
    public class Law : Effect
    {
        Discipline? Discipline { get; set; }

        public Law() { }

        private Law(Law law)
        {
            Discipline = law.Discipline?.Clone() ?? null;
        }

        public override void Apply()
        {
            throw new NotImplementedException();
        }

        public override Effect Clone()
        {
            return new Law(this);
        }
    }
}
