using System.Xml.Serialization;

namespace GameLogic.Goods.Factories
{
    [XmlInclude(typeof(GlassFactory)),
     XmlInclude(typeof(ClayFactory)),
     XmlInclude(typeof(PapirusFactory)),
     XmlInclude(typeof(StoneFactory)),
     XmlInclude(typeof(WoodFactory))]
    public abstract class GoodFactory
    {
        public abstract Good CreateGood();
    }
}
