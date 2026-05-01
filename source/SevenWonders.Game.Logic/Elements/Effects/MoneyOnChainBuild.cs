using SevenWonders.Game.Logic.Events.GameEvents;

namespace SevenWonders.Game.Logic.Elements.Effects
{
    public class MoneyOnChainBuild : Effect
    {
        public GetMoney MoneyToGet { get; set; }

        public MoneyOnChainBuild()
        {
            MoneyToGet = new GetMoney();
        }
        private MoneyOnChainBuild(MoneyOnChainBuild moneyOnChainBuild)
        {
            MoneyToGet = moneyOnChainBuild.MoneyToGet.Clone();
        }

        public override MoneyOnChainBuild Clone()
        {
            return new MoneyOnChainBuild(this);
        }

        public override void Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.OnCardBuilt += GetMoneyOnChainBuild;
        }

        public override void Unapply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.OnCardBuilt -= GetMoneyOnChainBuild;
        }

        private Task GetMoneyOnChainBuild(Player owner, OnCardBuilt args)
        {
            if (args.ChainBuildUsed)
            {
                owner.Money += MoneyToGet.Money;
            }
            return Task.CompletedTask;
        }
    }
}
