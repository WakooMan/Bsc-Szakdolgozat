namespace GameLogic.Elements.Effects
{
    public class GetMoneyForWonders : Effect
    {
        public int MoneyPerWonder { get; set; }
        public int GetTotalMoney(Player owner)
        {
            return MoneyPerWonder * owner.Wonders.Count(wonder => wonder.HasBeenBuilt);
        }

        public GetMoneyForWonders() { }

        private GetMoneyForWonders(GetMoneyForWonders getMoneyForWonders)
        {
            MoneyPerWonder = getMoneyForWonders.MoneyPerWonder;
        }

        public override GetMoneyForWonders Clone()
        {
            return new GetMoneyForWonders(this);
        }

        public override void Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.Money += GetTotalMoney(owner);
        }

    }
}
