using System.Xml.Serialization;

namespace GameLogic.Goods.Products
{
    [XmlInclude(typeof(Papirus)),
     XmlInclude(typeof(Glass))]
    public abstract class Product : Good
    { }
}
