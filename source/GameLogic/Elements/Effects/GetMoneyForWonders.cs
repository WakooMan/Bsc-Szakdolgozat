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

        public override Task Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.Money += MoneyPerWonder * owner.Wonders.Count(wonder => wonder.HasBeenBuilt);
            return Task.CompletedTask;
        }

    }
}
