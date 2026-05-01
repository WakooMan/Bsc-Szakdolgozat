using SevenWonders.Game.Logic.Elements.Goods.Products;

namespace SevenWonders.Game.Logic.Elements.GameCards
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

        public override void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            foreach (Product product in CreatedProducts)
            {
                playerProperties.AddGood(product);
            }
        }
    }
}
