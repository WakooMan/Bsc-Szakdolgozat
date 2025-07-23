using GameLogic.Elements.Effects;
using GameLogic.GameStates;

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

        public override void OnCardBuilt(PlayingState game)
        {
            Effects.ForEach(effect => effect.Apply(game));
        }

        public override YellowCard Clone()
        {
            return new YellowCard(this);
        }
    }
}
