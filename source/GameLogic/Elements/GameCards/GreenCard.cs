using GameLogic.Elements.Disciplines;
using GameLogic.Elements.Effects;

namespace GameLogic.Elements.GameCards
{
    public class GreenCard : Card
    {
        public Discipline Discipline { get; set; }
        public VictoryPoints Point { get; set; }
        public GreenCard() : base()
        {
            Discipline = new DefaultDiscipline();
            Point = new VictoryPoints();
        }

        private GreenCard(GreenCard greenCard) : base(greenCard)
        {
            Discipline = greenCard.Discipline.Clone();
            Point = greenCard.Point.Clone();
        }

        public override GreenCard Clone()
        {
            return new GreenCard(this);
        }

        public override async Task OnBuilt(IGameContext gameContext, int playerId)
        {
            await Point.Apply(gameContext, playerId);
            await Discipline.Apply(gameContext, playerId);
        }

        public override async Task OnDestroyed(IGameContext gameContext, int playerId)
        {
            await Point.Unapply(gameContext, playerId);
            await Discipline.Unapply(gameContext, playerId);
        }
    }
}
