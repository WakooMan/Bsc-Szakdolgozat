using GameLogic.Elements.Effects;

namespace GameLogic.Elements.Wonders
{
    public class Wonder
    {
        public string Name { get; set; }
        public List<Effect> Effects { get; set; }

        public Wonder()
        {
            Effects = new List<Effect>();
        }

        private Wonder(Wonder wonder)
        {
            Name = wonder.Name;
            Effects = wonder.Effects.Select(effect => effect.Clone()).ToList();
        }

        public Wonder Clone()
        {
            return new Wonder(this);
        }
    }
}
