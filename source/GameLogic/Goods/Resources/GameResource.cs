using System.Xml.Serialization;

namespace GameLogic.Goods.Resources
{
    [XmlInclude(typeof(Clay)),
     XmlInclude(typeof(Stone)),
     XmlInclude(typeof(Wood))]
    public abstract class GameResource : Good
    { }
}
