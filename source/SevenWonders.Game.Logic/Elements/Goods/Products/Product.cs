using SevenWonders.Game.Logic.Elements.Goods;
using SevenWonders.Game.Logic.Elements.Goods.Resources;
using System.Xml.Serialization;

namespace SevenWonders.Game.Logic.Elements.Goods.Products
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
