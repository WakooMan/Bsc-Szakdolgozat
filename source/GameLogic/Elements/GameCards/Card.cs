using GameLogic.Ages;
using GameLogic.Elements.Goods;
using GameLogic.Handlers;
using System.Xml.Serialization;

namespace GameLogic.Elements.GameCards
{
    [XmlInclude(typeof(BrownCard)),
     XmlInclude(typeof(BlueCard)),
     XmlInclude(typeof(GrayCard)),
     XmlInclude(typeof(GreenCard)),
     XmlInclude(typeof(PurpleCard)),
     XmlInclude(typeof(RedCard)),
     XmlInclude(typeof(YellowCard))]
    public abstract class Card : IBuildable
    {
        public List<Good> GoodCost { get; set; }
        public int MoneyCost { get; set; }
        public string Name { get; set; }
        public string PreviousBuilding { get; set; }
        public AgesEnum Age { get; set; }

        public string BuildingType => GetType().Name;

        public abstract Card Clone();
        public virtual int GetStrength()
        {
            return 0;
        }
        public virtual int GetVictoryPoints(Player player)
        {
            return 0;
        }

        public virtual List<Good> GetGoods()
        {
            return new List<Good>();
        }

        public virtual void OnBuilt(IGameContext gameContext)
        {

        }

        protected Card()
        {
            GoodCost = new List<Good>();
            Name = string.Empty;
            PreviousBuilding = string.Empty;
            Age = AgesEnum.None;
        }

        protected Card(Card card)
        {
            GoodCost = card.GoodCost.Select(g => g.Clone()).ToList();
            Name = card.Name;
            PreviousBuilding = card.PreviousBuilding;
            Age = card.Age;
            MoneyCost = card.MoneyCost;
        }
    }
}
