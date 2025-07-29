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
            gameContext.EventManager.Subscribe(GameEventType.CardBuilt, (args) => GetMoneyOnChainBuild(player, args));
        }

        private void GetMoneyOnChainBuild(Player player, EventArgs args)
        {
            if (args is OnCardBuilt onCardBuilt && player == onCardBuilt.Builder && onCardBuilt.ChainBuildUsed)
            {
                player.Money += MoneyToGet.Money;
            }
        }
    }
}
