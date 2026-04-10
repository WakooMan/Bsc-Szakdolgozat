using GameLogic.Elements.Goods.Products;
using GameLogic.Events.GameEvents;

namespace GameLogic.Elements.GameCards
{
    public class GrayCard : Card
    {
        public List<Product> CreatedProducts { get; set; }
        public GrayCard() : base()
        {
            CreatedProducts = new List<Product>();
        }

        private GrayCard(GrayCard grayCard) : base(grayCard)
        {
            CreatedProducts = grayCard.CreatedProducts.Select(prod => prod.Clone()).ToList();
        }

        public override GrayCard Clone()
        {
            return new GrayCard(this);
        }

        public override Task OnBuilt(IGameContext gameContext, int playerId)
        {
            gameContext.EventManager.Subscribe<OnCalculatePlayerProperties>(OnCalculatePlayerProperties);
            return Task.CompletedTask;
        }

        public override Task OnDestroyed(IGameContext gameContext, int playerId)
        {
            gameContext.EventManager.Unsubscribe<OnCalculatePlayerProperties>(OnCalculatePlayerProperties);
            return Task.CompletedTask;
        }

        private void OnCalculatePlayerProperties(OnCalculatePlayerProperties properties)
        {
            if (properties.PlayerProperties.Player.HasCard(this))
            {
                foreach (Product product in CreatedProducts)
                {
                    properties.PlayerProperties.AddGood(product);
                }
            }
        }
    }
}
