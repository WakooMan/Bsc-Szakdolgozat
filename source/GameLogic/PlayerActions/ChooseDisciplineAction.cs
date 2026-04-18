using GameLogic.Elements;
using GameLogic.Elements.Disciplines;
using SevenWonders.Common;

namespace GameLogic.PlayerActions
{
    public class ChooseDisciplineAction: IPlayerAction
    {
        public string Name => m_discipline.GetType().Name;
        public ChooseDisciplineAction(Discipline discipline, Player owner, Player opponent, Func<IGameContext, Player, Player, Discipline, Task> setter)
        {
            ArgumentChecker.CheckNull(owner, nameof(owner));
            ArgumentChecker.CheckNull(opponent, nameof(opponent));
            ArgumentChecker.CheckNull(discipline, nameof(discipline));
            ArgumentChecker.CheckNull(setter, nameof(setter));

            m_owner = owner;
            m_opponent = opponent;
            m_discipline = discipline;
            m_setter = setter;
        }

        public async Task<bool> DoPlayerAction(IGameContext gameContext)
        {
            await m_setter(gameContext, m_owner, m_opponent, m_discipline);
            return true;
        }

        public Task<bool> CanPerform(IGameContext gameContext)
        {
            return Task.FromResult(true);
        }

        private readonly Discipline m_discipline;
        private readonly Player m_owner;
        private readonly Player m_opponent;
        private readonly Func<IGameContext, Player, Player, Discipline, Task> m_setter;
    }
}
