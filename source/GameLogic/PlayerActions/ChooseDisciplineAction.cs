using GameLogic.Elements.Disciplines;
using SevenWonders.Common;

namespace GameLogic.PlayerActions
{
    public class ChooseDisciplineAction: IPlayerAction
    {
        public string Name => m_discipline.GetType().Name;
        public ChooseDisciplineAction() { }
        public ChooseDisciplineAction(Discipline discipline, Action<Discipline> setter)
        {
            ArgumentChecker.CheckNull(discipline, nameof(discipline));
            ArgumentChecker.CheckNull(setter, nameof(setter));

            m_discipline = discipline;
            m_setter = setter;
        }

        public Task DoPlayerAction(IGameContext gameContext)
        {
            m_setter(m_discipline);
            return Task.CompletedTask;
        }

        public Task<bool> CanPerform(IGameContext gameContext)
        {
            return Task.FromResult(true);
        }

        private readonly Discipline m_discipline;
        private readonly Action<Discipline> m_setter;
    }
}
