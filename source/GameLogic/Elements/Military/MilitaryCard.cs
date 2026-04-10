using GameLogic.Elements.Effects;

namespace GameLogic.Elements.Military
{
    public class MilitaryCard
    {
        public string Name { get; set; }
        public int PlayerId { get; set; }
        public EnemyLoseMoney EnemyLoseMoney { get; set; }
        public VictoryPoints VictoryPoints { get; set; }
        public int IndexStart { get; set; }
        public int IndexEnd { get; set; }
        public bool FirstApply { get; set; }

        public MilitaryCard()
        {
            Name = string.Empty;
            EnemyLoseMoney = new EnemyLoseMoney();
            VictoryPoints = new VictoryPoints();
            FirstApply = true;
            PlayerId = 0;
        }

        public void Apply(IGameContext gameContext)
        {
            if (FirstApply)
            {
                EnemyLoseMoney.Apply(gameContext, PlayerId);
            }

            VictoryPoints.Apply(gameContext, PlayerId);
            FirstApply = false;
        }

        public void Unapply(IGameContext gameContext)
        {
            VictoryPoints.Unapply(gameContext, PlayerId);
        }
    }
}
