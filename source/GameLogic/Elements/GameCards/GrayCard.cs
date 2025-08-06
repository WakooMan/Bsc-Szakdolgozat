using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Products;

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

        public override List<Good> GetGoods()
        {
            return CreatedProducts.Select(prod => (Good)prod).ToList();
        }
    }
}
