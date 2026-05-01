namespace SevenWonders.Game.Logic.Elements.Effects
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

        public override void Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            opponent.Money -= Money;
        }
    }
}
