using GameLogic.GameStates;

namespace GameLogic.Elements.Effects
{
    public class VictoryPoints : Effect
    {
        public int Points { get; set; }

        public VictoryPoints()
        {
            Points = 0;
        }

        private VictoryPoints(VictoryPoints victoryPoints)
        {
            Points = victoryPoints.Points;
        }

        public override VictoryPoints Clone()
        {
            return new VictoryPoints(this);
        }
    }
}
