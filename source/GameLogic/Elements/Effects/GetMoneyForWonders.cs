using GameLogic.GameStates;

namespace GameLogic.Elements.Effects
{
    public class GetMoneyForWonders : Effect
    {
        public int MoneyPerWonder { get; set; }

        public GetMoneyForWonders() { }

        private GetMoneyForWonders(GetMoneyForWonders getMoneyForWonders)
        {
            MoneyPerWonder = getMoneyForWonders.MoneyPerWonder;
        }

        public override GetMoneyForWonders Clone()
        {
            return new GetMoneyForWonders(this);
        }

        public override void Apply(PlayingState game)
        {
            throw new NotImplementedException();
        }
    }
}
