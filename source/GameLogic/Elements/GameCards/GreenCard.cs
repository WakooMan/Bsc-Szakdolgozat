using GameLogic.Elements.Disciplines;
using GameLogic.Elements.Effects;

namespace GameLogic.Elements.GameCards
{
    public class GreenCard : Card
    {
        public Discipline Discipline { get; set; }
        public VictoryPoints Point { get; set; }
        public GreenCard() : base()
        { }

        private GreenCard(GreenCard greenCard) : base(greenCard)
        {
            Discipline = greenCard.Discipline.Clone();
            Point = greenCard.Point.Clone();
        }

        public override GreenCard Clone()
        {
            return new GreenCard(this);
        }

        public override int GetVictoryPoints(Player player)
        {
            return Point.Points;
        }
    }
}
