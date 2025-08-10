using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Resources;
using System.Xml.Serialization;

namespace GameLogic.Elements.Goods.Products
{
    [XmlInclude(typeof(Papirus)),
     XmlInclude(typeof(Glass))]
    public abstract class Product : Good
    {
        protected Product(Good good) : base(good)
        {
        }

        protected Product() :base() { }

        public override abstract Product Clone();
    }
}
