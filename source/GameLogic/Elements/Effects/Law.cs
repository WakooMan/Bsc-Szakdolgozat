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
            var list = new List<IPlayerAction>
            {
                new ChooseDisciplineAction(new Building(), SetDiscipline),
                new ChooseDisciplineAction(new Geography(), SetDiscipline),
                new ChooseDisciplineAction(new Healing(), SetDiscipline),
                new ChooseDisciplineAction(new Mechanics(), SetDiscipline),
                new ChooseDisciplineAction(new Physics(), SetDiscipline),
                new ChooseDisciplineAction(new Trading(), SetDiscipline),
                new ChooseDisciplineAction(new Writing(), SetDiscipline)
            };

            await gameContext.EventManager.PublishAsync(new OnChooseObjects("Choose Discipline", list.Select(action => action.Name).ToArray()));
            await gameContext.PlayerActionHandler.HandlePlayerActions(gameContext, gameContext.TurnHandler.CurrentPlayer, list);
            await gameContext.EventManager.PublishAsync(new OnObjectChosen(list.Select(action => action.Name).ToArray()));
        }

        private void SetDiscipline(Discipline discipline)
        {
            m_discipline = discipline;
        }

        private Discipline m_discipline;
    }
}
