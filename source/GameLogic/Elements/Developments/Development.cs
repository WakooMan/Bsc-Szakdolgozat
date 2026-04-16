using GameLogic.Elements.Effects;

namespace GameLogic.Elements.Modifiers
{
    public class Development
    {
        public string Name { get; set; }
        public List<Effect> Effects { get; set; }

        public Development()
        {
            Name = string.Empty;
            Effects = new List<Effect>();
        }

        private Development(Development development)
        {
            Name = development.Name;
            Effects = development.Effects.Select(eff => eff.Clone()).ToList();
        }

        public Development Clone()
        {
            return new Development(this);
        }

        public void OnDevelopmentEstablished(IGameContext gameContext, int playerId)
        {
            Effects.ForEach(effect => effect.Apply(gameContext, playerId));
        }

        public void OnDevelopmentRemoved(IGameContext gameContext, int playerId)
        {
            Effects.ForEach(effect => effect.Unapply(gameContext, playerId));
        }

        public async Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            foreach (Effect effect in Effects)
            {
                await effect.OnCalculatePlayerProperties(playerProperties);
            }
        }

        public async Task OnBeforeGameEnded(Player owner, Player opponent)
        {
            foreach (Effect effect in Effects)
            {
                await effect.OnBeforeGameEnded(owner, opponent);
            }
        }
    }
}
