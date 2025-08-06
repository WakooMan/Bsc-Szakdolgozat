using GameLogic.Elements.Effects;

namespace GameLogic.Elements.Military
{
    public class MilitaryCard
    {
        public EnemyLoseMoney EnemyLoseMoney { get; set; }
        public VictoryPoints VictoryPoints { get; set; }
        public int IndexStart { get; set; }
        public int IndexEnd { get; set; }

        public MilitaryCard()
        {
            EnemyLoseMoney = new EnemyLoseMoney();
            VictoryPoints = new VictoryPoints();
        }

        public void Apply(IGameContext gameContext)
        {
            EnemyLoseMoney.Apply(gameContext);
            VictoryPoints.Apply(gameContext);
        }
    }
}
