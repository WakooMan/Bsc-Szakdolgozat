using GameLogic.Events;

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

        public override void Apply(IGameContext gameContext)
        {
            Player player = gameContext.TurnHandler.CurrentPlayer;
            player.Money += MoneyPerCard * player.Cards.Where(card => card.CardType == CardType).Count();
        }

    }
}
