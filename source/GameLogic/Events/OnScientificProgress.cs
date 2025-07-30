using GameLogic.Elements;
using GameLogic.Elements.Disciplines;
using GameLogic.Interfaces;

namespace GameLogic.Events
{
    public class OnScientificProgress: EventArgs
    {
        public Player Player { get; set; }
        public Discipline Discipline { get; set; }
        public IPlayerActionReceiver PlayerActionReceiver { get; set; }

        public OnScientificProgress(Player player, Discipline discipline, IPlayerActionReceiver playerActionReceiver)
        {
            Player = player;    
            Discipline = discipline;
            PlayerActionReceiver = playerActionReceiver;
        }
    }
}
