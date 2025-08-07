using GameLogic.Elements.Disciplines;

namespace GameLogic.PlayerActions
{
    public class ChooseDisciplineAction: IPlayerAction
    {
        public ChooseDisciplineAction(Discipline discipline, Action<Discipline> setter)
        {
            m_discipline = discipline;
            m_setter = setter;
        }

        public void DoPlayerAction(IGameContext gameContext)
        {
            m_setter(m_discipline);
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return m_discipline is not null && m_setter is not null;
        }

        private readonly Discipline m_discipline;
        private readonly Action<Discipline> m_setter;
    }
}
