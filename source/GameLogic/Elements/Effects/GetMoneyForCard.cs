namespace GameLogic.Elements.Effects
{
    public class GetMoneyForCard : Effect
    {
        public string? CardType { get; set; }
        public int MoneyPerCard { get; set; }

        public GetMoneyForCard() { }
        private GetMoneyForCard(GetMoneyForCard getMoneyForCard)
        {
            CardType = getMoneyForCard.CardType;
            MoneyPerCard = getMoneyForCard.MoneyPerCard;
        }

        public override GetMoneyForCard Clone()
        {
            return new GetMoneyForCard(this);
        }

        public override void Apply()
        {
            throw new NotImplementedException();
        }
    }
}
