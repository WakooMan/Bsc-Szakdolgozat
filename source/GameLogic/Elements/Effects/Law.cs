using GameLogic.Elements.Disciplines;
using GameLogic.Interfaces;
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
            var list = new List<ChooseDisciplineAction>
            {
                new ChooseDisciplineAction(new Building(), SetDiscipline),
                new ChooseDisciplineAction(new Geography(), SetDiscipline),
                new ChooseDisciplineAction(new Healing(), SetDiscipline),
                new ChooseDisciplineAction(new Mechanics(), SetDiscipline),
                new ChooseDisciplineAction(new Physics(), SetDiscipline),
                new ChooseDisciplineAction(new Trading(), SetDiscipline),
                new ChooseDisciplineAction(new Writing(), SetDiscipline)
            };
            var playerAction = gameContext.PlayerActionReceiver.ReceivePlayerAction(gameContext.TurnHandler.CurrentPlayer, list.Select(action => new PlayerActionWrapper(action, action.CanPerform(gameContext).GetAwaiter().GetResult())).ToList());

            if (playerAction.CanPerform)
            {
                await playerAction.PlayerAction.DoPlayerAction(gameContext);
            }
        }

        private void SetDiscipline(Discipline discipline)
        {
            m_discipline = discipline;
        }

        private Discipline m_discipline;
    }
}
