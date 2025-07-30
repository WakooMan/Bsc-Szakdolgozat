using GameLogic.Elements.Disciplines;
using GameLogic.Elements.Effects;
using GameLogic.Events;

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

        public override void OnBuilt(IGameContext gameContext)
        {
            gameContext.EventManager.Publish(GameEventType.ScientificProgress, new OnScientificProgress(gameContext.TurnHandler.CurrentPlayer, Discipline, gameContext.PlayerActionReceiver));
            Point.Apply(gameContext);
        }
    }
}
