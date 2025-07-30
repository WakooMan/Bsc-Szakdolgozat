using GameLogic.Events;

namespace GameLogic.Elements.Effects
{
    public class MoneyOnChainBuild : Effect
    {
        public GetMoney MoneyToGet { get; set; }

        public MoneyOnChainBuild() { }
        private MoneyOnChainBuild(MoneyOnChainBuild moneyOnChainBuild)
        {
            MoneyToGet = moneyOnChainBuild.MoneyToGet.Clone();
        }

        public override MoneyOnChainBuild Clone()
        {
            return new MoneyOnChainBuild(this);
        }

        public override void Apply(IGameContext gameContext)
        {
            Player player = gameContext.TurnHandler.CurrentPlayer;
            gameContext.EventManager.Subscribe<OnCardBuilt>(GameEventType.CardBuilt, (args) => GetMoneyOnChainBuild(player, args));
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
