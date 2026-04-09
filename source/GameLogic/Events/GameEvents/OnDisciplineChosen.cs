using GameLogic.Elements.Disciplines;

namespace GameLogic.Events.GameEvents
{
    public class OnDisciplineChosen : GameEvent
    {
        public List<Discipline> Disciplines { get; }

        public OnDisciplineChosen(ICollection<Discipline> disciplines)
        {
            Disciplines = disciplines.ToList();
        }
    }
}
