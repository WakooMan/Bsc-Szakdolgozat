using SevenWonders.Game.Logic.Elements.Effects;

namespace SevenWonders.Game.Logic.Elements.GameCards
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

        public override void OnBuilt(IGameContext gameContext, Player owner, Player opponent)
        {
            foreach (var effect in Effects)
            {
                effect.Apply(gameContext, owner, opponent);
            }
        }

        public override void OnDestroyed(IGameContext gameContext, Player owner, Player opponent)
        {
            foreach (var effect in Effects)
            {
                effect.Unapply(gameContext, owner, opponent);
            }
        }

        public override YellowCard Clone()
        {
            return new YellowCard(this);
        }

        public override void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            foreach (Effect effect in Effects)
            {
                effect.OnCalculatePlayerProperties(playerProperties);
            }
        }
    }
}
