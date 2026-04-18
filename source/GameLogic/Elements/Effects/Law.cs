using GameLogic.Elements.Disciplines;
using GameLogic.Events.GameEvents;
using GameLogic.PlayerActions;

namespace GameLogic.Elements.Effects
{
    public class Law : Effect
    {
        public Law()
        {
            m_discipline = new DefaultDiscipline();
        }

        private Law(Law law)
        {
            m_discipline = law.m_discipline?.Clone();
        }

        public override Law Clone()
        {
            return new Law(this);
        }

        public override async Task Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            var list = new List<IPlayerAction>
            {
                new ChooseDisciplineAction(new Building(), owner, opponent, SetDiscipline),
                new ChooseDisciplineAction(new Geography(), owner, opponent, SetDiscipline),
                new ChooseDisciplineAction(new Healing(), owner, opponent, SetDiscipline),
                new ChooseDisciplineAction(new Mechanics(), owner, opponent, SetDiscipline),
                new ChooseDisciplineAction(new Physics(), owner, opponent, SetDiscipline),
                new ChooseDisciplineAction(new Trading(), owner, opponent, SetDiscipline),
                new ChooseDisciplineAction(new Writing(), owner, opponent, SetDiscipline)
            };

            await gameContext.EventManager.PublishAsync(new OnChooseObjects("Válassz tudományos jelképet", list.Select(action => action.Name).ToArray(), false));

            await gameContext.PlayerActionHandler.HandlePlayerActions(gameContext, owner, list);
            await gameContext.EventManager.PublishAsync(new OnObjectChosen(list.Select(action => action.Name).ToArray(), false));
        }

        public override Task Unapply(IGameContext gameContext, Player owner, Player opponent)
        {
            m_discipline = null;
            return Task.CompletedTask;
        }

        public override async Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            if(m_discipline is not null)
            {
                await m_discipline.OnCalculatePlayerProperties(playerProperties);
            }
        }

        private async Task SetDiscipline(IGameContext gameContext, Player owner, Player opponent, Discipline discipline)
        {
            m_discipline = discipline;
            await m_discipline.Apply(gameContext, owner, opponent);
        }

        private Discipline? m_discipline;
    }
}
