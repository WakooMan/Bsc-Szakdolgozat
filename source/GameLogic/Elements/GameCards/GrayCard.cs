using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Elements.GameCards
{
    public class GrayCard : Card
    {
        public List<Product> CreatedProducts { get; set; }
        public GrayCard() : base()
        { }

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
