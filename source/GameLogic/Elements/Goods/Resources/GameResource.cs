using GameLogic.Elements.Goods;
using System.Xml.Serialization;

namespace GameLogic.Elements.Goods.Resources
{
    [XmlInclude(typeof(Clay)),
     XmlInclude(typeof(Stone)),
     XmlInclude(typeof(Wood))]
    public abstract class GameResource : Good
    {
        public override abstract GameResource Clone();
    }
}
