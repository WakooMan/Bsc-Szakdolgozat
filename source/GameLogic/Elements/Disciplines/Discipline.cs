using System.Xml.Serialization;

namespace GameLogic.Elements.Disciplines
{
    [XmlInclude(typeof(Building)),
     XmlInclude(typeof(Geography)),
     XmlInclude(typeof(Healing)),
     XmlInclude(typeof(Mechanics)),
     XmlInclude(typeof(Physics)),
     XmlInclude(typeof(Trading)),
     XmlInclude(typeof(Writing)),]
    public abstract class Discipline
    {
    }
}
