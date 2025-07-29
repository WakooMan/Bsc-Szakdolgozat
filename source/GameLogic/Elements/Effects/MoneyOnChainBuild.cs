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
            gameContext.EventManager.Subscribe(GameEventType.CardBuilt, GetMoneyOnChainBuild);
        }

        private void GetMoneyOnChainBuild(EventArgs args)
        {

        }
    }
}
