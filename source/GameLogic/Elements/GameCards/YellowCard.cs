using GameLogic.Elements.Effects;

namespace GameLogic.Elements.GameCards
{
    public class YellowCard : Card
    {
        public List<Effect> Effects { get; set; }

        public YellowCard() : base()
        {
            Effects = new List<Effect>();
        }

        private YellowCard(YellowCard yellowCard) : base(yellowCard)
        {
            Effects = yellowCard.Effects.Select(act => act.Clone()).ToList();
        }

        public override async Task OnBuilt(IGameContext gameContext, int playerId)
        {
            foreach (var effect in Effects)
            {
                await effect.Apply(gameContext, playerId);
            }
        }

        public override async Task OnDestroyed(IGameContext gameContext, int playerId)
        {
            foreach (var effect in Effects)
            {
                await effect.Unapply(gameContext, playerId);
            }
        }

        public override YellowCard Clone()
        {
            return new YellowCard(this);
        }

        public override async Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            foreach (Effect effect in Effects)
            {
                await effect.OnCalculatePlayerProperties(playerProperties);
            }
        }
    }
}
