using GameLogic.Elements.Disciplines;
using SevenWonders.Common;

namespace GameLogic.PlayerActions
{
    public class ChooseDisciplineAction: IPlayerAction
    {
        public string Name => m_discipline.GetType().Name;
        public ChooseDisciplineAction() { }
        public ChooseDisciplineAction(Discipline discipline, Func<IGameContext, Discipline, int, Task> setter, int playerId)
        {
            ArgumentChecker.CheckNull(discipline, nameof(discipline));
            ArgumentChecker.CheckNull(setter, nameof(setter));

            m_discipline = discipline;
            m_setter = setter;
            m_playerId = playerId;
        }

        public async Task<bool> DoPlayerAction(IGameContext gameContext)
        {
            await m_setter(gameContext, m_discipline, m_playerId);
            return true;
        }

        public Task<bool> CanPerform(IGameContext gameContext)
        {
            return Task.FromResult(true);
        }

        private readonly Discipline m_discipline;
        private readonly Func<IGameContext, Discipline, int, Task> m_setter;
        private readonly int m_playerId;
    }
}
