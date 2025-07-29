using GameLogic.Elements.Effects;

namespace GameLogic.Elements.Modifiers
{
    public class Development
    {
        public string Name { get; set; }
        public List<Effect> Effects { get; set; }

        public Development() { }

        private Development(Development development)
        {
            Name = development.Name;
            Effects = development.Effects.Select(eff => eff.Clone()).ToList();
        }

        public Development Clone()
        {
            return new Development(this);
        }

        public void OnDevelopmentEstablished(IGameContext gameContext)
        {
            Effects.ForEach(effect => effect.Apply(gameContext));
        }
    }
}
