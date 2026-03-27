using GameLogic.Elements.Disciplines;
using GameLogic.Elements.Effects;
using GameLogic.Events;
using GameLogic.Events.GameEvents;

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

        public override int GetVictoryPoints(Player player)
        {
            return Point.Points;
        }

        public override async Task OnBuilt(IGameContext gameContext)
        {
            await gameContext.EventManager.PublishAsync(new OnScientificProgress(gameContext.TurnHandler.CurrentPlayer, Discipline, gameContext.PlayerActionReceiver));
            await Point.Apply(gameContext);
        }
    }
}
