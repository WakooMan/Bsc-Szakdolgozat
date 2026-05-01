using SevenWonders.Game.Logic.Elements.Goods;
using System.Xml.Serialization;

namespace SevenWonders.Game.Logic.Elements.Goods.Resources
{
    [XmlInclude(typeof(Clay)),
     XmlInclude(typeof(Stone)),
     XmlInclude(typeof(Wood))]
    public abstract class GameResource : Good
    {
        protected GameResource(Good good) : base(good)
        {
        }

        protected GameResource() : base() { }

        public override abstract GameResource Clone();
    }
}
