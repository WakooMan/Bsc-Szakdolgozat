using GameLogic.Elements.Goods;
using System.Xml.Serialization;

namespace GameLogic.Elements.Goods.Products
{
    [XmlInclude(typeof(Papirus)),
     XmlInclude(typeof(Glass))]
    public abstract class Product : Good
    { }
}
