using SevenWonders.Game.Logic.Elements.Goods.Products;
using SevenWonders.Game.Logic.Elements.Goods.Resources;
using System.Xml.Serialization;

namespace SevenWonders.Game.Logic.Elements.Goods
{
    [XmlInclude(typeof(GameResource)),
     XmlInclude(typeof(Product))]
    public abstract class Good
    {
        public int Amount { get; set; }

        protected Good(Good good)
        {
            Amount = good.Amount;
        }

        protected Good()
        {
            Amount = 0;
        }

        public abstract Good Clone();
    }
}
