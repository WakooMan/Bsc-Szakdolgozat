using SevenWonders.Game.Logic.Elements.Disciplines;
using SevenWonders.Game.Logic.Elements.Effects;

namespace SevenWonders.Game.Logic.Elements.GameCards
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

        public override void OnBuilt(IGameContext gameContext, Player owner, Player opponent)
        {
            Discipline.Apply(gameContext, owner, opponent);
        }

        public override void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            Point.OnCalculatePlayerProperties(playerProperties);
            Discipline.OnCalculatePlayerProperties(playerProperties);
        }
    }
}
