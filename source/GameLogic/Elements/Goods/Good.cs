using GameLogic.Elements.Goods.Products;
using GameLogic.Elements.Goods.Resources;
using System.Xml.Serialization;

namespace GameLogic.Elements.Goods
{
    [XmlInclude(typeof(GameResource)),
     XmlInclude(typeof(Product))]
    public abstract class Good : IEquatable<Good>
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
        public abstract bool Equals(Good? other);
    }
}
