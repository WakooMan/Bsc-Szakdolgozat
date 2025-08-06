using GameLogic.Elements.Disciplines;
using GameLogic.PlayerActions;

namespace GameLogic.Elements.Effects
{
    public class Law : Effect
    {
        public Discipline Discipline { get; set; }

        public Law()
        {
            Discipline = new DefaultDiscipline();
        }

        private Law(Law law)
        {
            Discipline = law.Discipline.Clone();
        }

        public override Effect Clone()
        {
            return new Law(this);
        }

        public override void Apply(IGameContext gameContext)
        {
            Discipline = gameContext.PlayerActionReceiver.ReceivePlayerAction(gameContext.TurnHandler.CurrentPlayer, [
                new ChooseDisciplineAction(new Building()),
                new ChooseDisciplineAction(new Geography()),
                new ChooseDisciplineAction(new Healing()),
                new ChooseDisciplineAction(new Mechanics()),
                new ChooseDisciplineAction(new Physics()),
                new ChooseDisciplineAction(new Trading()),
                new ChooseDisciplineAction(new Writing())]).Discipline;
        }
    }
}
