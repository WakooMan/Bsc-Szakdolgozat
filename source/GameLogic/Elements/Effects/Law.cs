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

        public override async Task Apply(IGameContext gameContext, int playerId)
        {
            Player player = gameContext.TurnHandler.GetPlayer(playerId);
            var list = new List<IPlayerAction>
            {
                new ChooseDisciplineAction(new Building(), SetDiscipline, playerId),
                new ChooseDisciplineAction(new Geography(), SetDiscipline, playerId),
                new ChooseDisciplineAction(new Healing(), SetDiscipline, playerId),
                new ChooseDisciplineAction(new Mechanics(), SetDiscipline, playerId),
                new ChooseDisciplineAction(new Physics(), SetDiscipline, playerId),
                new ChooseDisciplineAction(new Trading(), SetDiscipline, playerId),
                new ChooseDisciplineAction(new Writing(), SetDiscipline, playerId)
            };

            await gameContext.EventManager.PublishAsync(new OnChooseObjects("Choose Discipline", list.Select(action => action.Name).ToArray(), false));

            await gameContext.PlayerActionHandler.HandlePlayerActions(gameContext, player, list);
            await gameContext.EventManager.PublishAsync(new OnObjectChosen(list.Select(action => action.Name).ToArray(), false));
        }

        public override async Task Unapply(IGameContext gameContext, int playerId)
        {
            await m_discipline.Unapply(gameContext, playerId);
        }

        private async Task SetDiscipline(IGameContext gameContext, Discipline discipline, int playerId)
        {
            m_discipline = discipline;
            await m_discipline.Apply(gameContext, playerId);
        }

        private Discipline m_discipline;
    }
}
