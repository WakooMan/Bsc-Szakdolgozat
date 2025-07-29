using GameLogic.Elements.Effects;
using GameLogic.Events;

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

        public override void OnBuilt(IGameContext gameContext)
        {
            Effects.ForEach(effect => effect.Apply(gameContext));
        }

        public override YellowCard Clone()
        {
            return new YellowCard(this);
        }
    }
}
