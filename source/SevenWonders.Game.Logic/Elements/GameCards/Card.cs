using SevenWonders.Game.Logic.Ages;
using SevenWonders.Game.Logic.Elements.Goods;
using SevenWonders.Game.Logic.Handlers;
using System.Xml.Serialization;

namespace SevenWonders.Game.Logic.Elements.GameCards
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
        public int ID { get; set; }
        public string Name { get; set; }
        public string PreviousBuilding { get; set; }
        public bool HasChainChild { get; set; }
        public AgesEnum Age { get; set; }

        public string BuildingType => GetType().Name;

        public abstract Card Clone();

        public virtual void OnBuilt(IGameContext gameContext, Player owner, Player opponent)
        {
        }

        public virtual void OnDestroyed(IGameContext gameContext, Player owner, Player opponent)
        {
        }

        public abstract void OnCalculatePlayerProperties(PlayerProperties playerProperties);

        protected Card()
        {
            GoodCost = new List<Good>();
            Name = string.Empty;
            PreviousBuilding = string.Empty;
            HasChainChild = false;
            Age = AgesEnum.None;
        }

        protected Card(Card card)
        {
            GoodCost = card.GoodCost.Select(g => g.Clone()).ToList();
            Name = card.Name;
            PreviousBuilding = card.PreviousBuilding;
            HasChainChild = card.HasChainChild;
            Age = card.Age;
            MoneyCost = card.MoneyCost;
        }
    }
}
