using GameLogic.GameStates;

namespace GameLogic.Elements.Effects
{
    public class VictoryPoints : Effect
    {
        public int Points { get; set; }

        public VictoryPoints() { }

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
