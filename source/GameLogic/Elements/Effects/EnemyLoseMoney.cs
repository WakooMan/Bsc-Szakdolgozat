namespace GameLogic.Elements.Effects
{
    public class EnemyLoseMoney : Effect
    {
        public int Money { get; set; }

        public EnemyLoseMoney() { }

        private EnemyLoseMoney(EnemyLoseMoney enemyLoseMoney)
        {
            Money = enemyLoseMoney.Money;
        }

        public override EnemyLoseMoney Clone()
        {
            return new EnemyLoseMoney(this);
        }

        public override Task Apply(IGameContext gameContext)
        {
            gameContext.TurnHandler.OpponentPlayer.Money -= Money;
            return Task.CompletedTask;
        }
    }
}
