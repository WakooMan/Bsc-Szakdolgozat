using GameLogic.Elements.Disciplines;
using GameLogic.Events.GameEvents;
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

        public override Law Clone()
        {
            return new Law(this);
        }

        public override async Task Apply(IGameContext gameContext)
        {
            var list = new List<Discipline>
            {
                new Building(),
                new Geography(),
                new Healing(),
                new Mechanics(),
                new Physics(),
                new Trading(),
                new Writing()
            };

            await gameContext.EventManager.PublishAsync(new OnChooseDiscipline(list));
            await gameContext.PlayerActionHandler.HandlePlayerActionsCompleted(gameContext, gameContext.TurnHandler.CurrentPlayer, list.Select(discipline => (IPlayerAction)new ChooseDisciplineAction(discipline, SetDiscipline)).ToList());
            await gameContext.EventManager.PublishAsync(new OnDisciplineChosen(list));
        }

        private void SetDiscipline(Discipline discipline)
        {
            m_discipline = discipline;
        }

        private Discipline m_discipline;
    }
}
