using GameLogic.Goods.Products;
using GameLogic.Goods.Resources;
using System.Xml.Serialization;

namespace GameLogic.Goods
{
    [XmlInclude(typeof(GameResource)),
     XmlInclude(typeof(Product))]
    public abstract class Good
    { }
}
