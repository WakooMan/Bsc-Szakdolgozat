using GameLogic.Elements.Disciplines;

namespace GameLogic.Events.GameEvents
{
    public class OnChooseDiscipline: GameEvent
    {
        public List<Discipline> Disciplines { get; }

        public OnChooseDiscipline(ICollection<Discipline> disciplines)
        {
            Disciplines = disciplines.ToList();
        }
    }
}
