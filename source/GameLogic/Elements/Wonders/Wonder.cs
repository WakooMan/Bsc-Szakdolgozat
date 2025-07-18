using GameLogic.Elements.Effects;

namespace GameLogic.Elements.Wonders
{
    public class Wonder
    {
        public string Name { get; set; }
        public List<Effect> Effects { get; set; }

        public bool HasBeenBuilt { get; set; }

        public Wonder()
        {
            Effects = new List<Effect>();
            HasBeenBuilt = false;
        }

        private Wonder(Wonder wonder)
        {
            Name = wonder.Name;
            Effects = wonder.Effects.Select(effect => effect.Clone()).ToList();
            HasBeenBuilt = wonder.HasBeenBuilt;
        }

        public Wonder Clone()
        {
            return new Wonder(this);
        }
    }
}
