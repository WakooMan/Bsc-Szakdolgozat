using GameLogic.GameStates;

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

        public override void Apply(PlayingState game)
        {
            throw new NotImplementedException();
        }

        public override MoneyOnChainBuild Clone()
        {
            return new MoneyOnChainBuild(this);
        }
    }
}
