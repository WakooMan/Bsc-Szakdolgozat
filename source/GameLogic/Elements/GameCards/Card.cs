using GameLogic.Ages;
using System.Xml.Serialization;
using GameLogic.Elements.Goods;

namespace GameLogic.Elements.GameCards
{
    [XmlInclude(typeof(BrownCard)),
     XmlInclude(typeof(BlueCard)),
     XmlInclude(typeof(GrayCard)),
     XmlInclude(typeof(GreenCard)),
     XmlInclude(typeof(PurpleCard)),
     XmlInclude(typeof(RedCard)),
     XmlInclude(typeof(YellowCard))]
    public abstract class Card
    {
        public List<Good> GoodCost { get; set; }
        public int MoneyCost { get; set; }
        public string Name { get; set; }
        public string PreviousBuilding { get; set; }
        public AgesEnum Age { get; set; }

        protected Card()
        {
            GoodCost = new List<Good>();
            Name = string.Empty;
            PreviousBuilding = string.Empty;
        }
    }
}
