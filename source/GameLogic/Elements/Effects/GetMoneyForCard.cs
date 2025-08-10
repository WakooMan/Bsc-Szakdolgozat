using GameLogic.Events;

namespace GameLogic.Elements.Effects
{
    public class GetMoneyForCard : Effect
    {
        public string CardType { get; set; }
        public int MoneyPerCard { get; set; }

        public GetMoneyForCard()
        {
            CardType = string.Empty;
            MoneyPerCard = 0;
        }
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
            player.Money += MoneyPerCard * player.Cards.Count(card => card.BuildingType == CardType);
        }

    }
}
