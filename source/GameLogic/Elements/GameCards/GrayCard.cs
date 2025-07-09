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
    }
}
