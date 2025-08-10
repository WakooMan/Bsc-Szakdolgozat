using System.Xml.Serialization;

namespace GameLogic.Elements.Goods.Factories
{
    [XmlInclude(typeof(GlassFactory)),
     XmlInclude(typeof(ClayFactory)),
     XmlInclude(typeof(PapirusFactory)),
     XmlInclude(typeof(StoneFactory)),
     XmlInclude(typeof(WoodFactory))]
    public abstract class GoodFactory
    {
        public abstract Type GoodType { get; }
        public abstract Good CreateGood();
    }
}
