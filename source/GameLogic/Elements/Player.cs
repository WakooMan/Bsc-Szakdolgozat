using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods;
using GameLogic.Elements.Modifiers;
using GameLogic.Elements.Wonders;
using GameLogic.GameStructures;
using System.Xml.Serialization;

namespace GameLogic.Elements
{
    public class Player
    {
        public string Name { get; set; }
        public List<Wonder> Wonders { get; set; }
        public List<Card> Cards { get; set; }
        public List<Development> Developments { get; set; }

        [XmlIgnore]
        public ICardNode? PickedCard { get; set; }
        public int Money { get; set; }
        public List<Good> Goods => Cards.Select(card => card.GetGoods()).Aggregate((a, b) => a.Union(b).ToList());
        public int Strength => Cards.Select(card => card.GetStrength()).Sum();
        public int VictoryPoints => Cards.Select(card => card.GetVictoryPoints(this)).Sum();

        public Player()
        {
            Name = "";
            Wonders = new List<Wonder>();
            Cards = new List<Card>();
            Developments = new List<Development>();
            Money = 0;
        }

        public Player(string name)
        {
            Name = name;
            Wonders = new List<Wonder>();
            Cards = new List<Card>();
            Developments = new List<Development>();
            Money = 0;
        }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }
    }
}
