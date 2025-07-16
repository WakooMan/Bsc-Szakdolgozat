using GameLogic.Elements.Goods.Products;
using GameLogic.Elements.Goods.Resources;
using System.Xml.Serialization;

namespace GameLogic.Elements.Goods
{
    [XmlInclude(typeof(GameResource)),
     XmlInclude(typeof(Product))]
    public abstract class Good
    {
        public abstract Good Clone();
    }
}
