using GameLogic.Elements.Disciplines;
using SevenWonders.Common;

namespace GameLogic.PlayerActions
{
    public class ChooseDisciplineAction: IPlayerAction
    {
        public ChooseDisciplineAction(Discipline discipline, Action<Discipline> setter)
        {
            ArgumentChecker.CheckNull(discipline, nameof(discipline));
            ArgumentChecker.CheckNull(setter, nameof(setter));

            m_discipline = discipline;
            m_setter = setter;
        }

        public void DoPlayerAction(IGameContext gameContext)
        {
            m_setter(m_discipline);
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return true;
        }

        private readonly Discipline m_discipline;
        private readonly Action<Discipline> m_setter;
    }
}
