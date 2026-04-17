using GameLogic.Events.GameEvents;

namespace GameLogic.Elements.Effects
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

        public override Task Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.OnCardBuilt += GetMoneyOnChainBuild;
            return Task.CompletedTask;
        }

        public override Task Unapply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.OnCardBuilt -= GetMoneyOnChainBuild;
            return Task.CompletedTask;
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
