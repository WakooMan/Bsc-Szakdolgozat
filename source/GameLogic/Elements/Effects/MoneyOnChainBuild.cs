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

        public override Task Apply(IGameContext gameContext, int playerId)
        {
            Player player = gameContext.TurnHandler.CurrentPlayer;
            gameContext.EventManager.Subscribe<OnCardBuilt>((args) => GetMoneyOnChainBuild(player, args));
            return Task.CompletedTask;
        }

        private void GetMoneyOnChainBuild(Player player, OnCardBuilt args)
        {
            if (player == args.Builder && args.ChainBuildUsed)
            {
                player.Money += MoneyToGet.Money;
            }
        }
    }
}
