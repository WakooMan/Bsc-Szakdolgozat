using GameLogic.Elements;
using GameLogic.Elements.Disciplines;
using GameLogic.Interfaces;

namespace GameLogic.Events.GameEvents
{
    public class OnScientificProgress: GameEvent
    {
        public int PlayerId { get; }
        public IReadOnlyDictionary<Type, int> Disciplines { get; }
        public Discipline Discipline { get; }
        public IPlayerActionReceiver PlayerActionReceiver { get; }

        public OnScientificProgress(int playerId, IReadOnlyDictionary<Type, int> disciplines, Discipline discipline, IPlayerActionReceiver playerActionReceiver)
        {
            PlayerId = playerId;    
            Discipline = discipline;
            PlayerActionReceiver = playerActionReceiver;
            Disciplines = disciplines;
        }
    }
}
