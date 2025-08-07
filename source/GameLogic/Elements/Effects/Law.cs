using GameLogic.Elements.Disciplines;
using GameLogic.PlayerActions;

namespace GameLogic.Elements.Effects
{
    public class Law : Effect
    {
        public Discipline Discipline => m_discipline;

        public Law()
        {
            m_discipline = new DefaultDiscipline();
        }

        private Law(Law law)
        {
            m_discipline = law.m_discipline.Clone();
        }

        public override Effect Clone()
        {
            return new Law(this);
        }

        public override void Apply(IGameContext gameContext)
        {
            IPlayerAction playerAction = gameContext.PlayerActionReceiver.ReceivePlayerAction(gameContext.TurnHandler.CurrentPlayer, [
                new ChooseDisciplineAction(new Building(), SetDiscipline),
                new ChooseDisciplineAction(new Geography(), SetDiscipline),
                new ChooseDisciplineAction(new Healing(), SetDiscipline),
                new ChooseDisciplineAction(new Mechanics(), SetDiscipline),
                new ChooseDisciplineAction(new Physics(), SetDiscipline),
                new ChooseDisciplineAction(new Trading(), SetDiscipline),
                new ChooseDisciplineAction(new Writing(), SetDiscipline)]);

            if (playerAction.CanPerform(gameContext))
            {
                playerAction.DoPlayerAction(gameContext);
            }
        }

        private void SetDiscipline(Discipline discipline)
        {
            m_discipline = discipline;
        }

        private Discipline m_discipline;
    }
}
