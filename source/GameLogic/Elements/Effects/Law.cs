using GameLogic.Elements.Disciplines;

namespace GameLogic.Elements.Effects
{
    public class Law : Effect
    {
        public Physics Physics { get; set; }
        public Law()
        {
            Physics = new Physics();
        }

        private Law(Law law)
        {
            Physics = law.Physics.Clone();
        }

        public override Law Clone()
        {
            return new Law(this);
        }

        public override void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            playerProperties.AddDiscipline(Physics);
        }
    }
}
