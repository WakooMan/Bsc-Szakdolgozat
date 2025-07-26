using GameLogic.Ages;
using System.Xml.Serialization;
using GameLogic.Elements.Goods;
using GameLogic.GameStates;
using GameLogic.Elements.Effects;
using GameLogic.Events;
using GameLogic.Handlers;

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

        public virtual List<Effect> GetEffects()
        {
            return new List<Effect>();
        }

        public virtual void OnBuilt(Player player, IEventManager eventManager)
        {

        }

        protected Card()
        {
            GoodCost = new List<Good>();
            Name = string.Empty;
            PreviousBuilding = string.Empty;
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
