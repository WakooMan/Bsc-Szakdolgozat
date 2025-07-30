using GameLogic.Elements.Disciplines;
using GameLogic.Elements.Effects;
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
        public Dictionary<Type, int> Disciplines
        {
            get
            {
                Dictionary<Type, int> result = new Dictionary<Type, int>();
                Wonders.ForEach(wonder => wonder.Effects.Where(effect => effect is Law).Select(effect => (Law)effect).ToList().ForEach(law => { if (result.ContainsKey(law.Discipline.GetType())) { result[law.Discipline.GetType()] += 1; } else { result[law.Discipline.GetType()] = 1; } }));
                Cards.Where(card => card is GreenCard).Select(card => (GreenCard)card).ToList().ForEach(card => { if (result.ContainsKey(card.Discipline.GetType())) { result[card.Discipline.GetType()] += 1; } else { result[card.Discipline.GetType()] = 1; } });
                Developments.ForEach(dev => dev.Effects.Where(effect => effect is Law).Select(effect => (Law)effect).ToList().ForEach(law => { if (result.ContainsKey(law.Discipline.GetType())) { result[law.Discipline.GetType()] += 1; } else { result[law.Discipline.GetType()] = 1; } }));
                return result;
            }
        }

        [XmlIgnore]
        public ICardNode? PickedCard { get; set; }
        public int Money { get; set; }
        public Dictionary<Type, Good> Goods
        { 
            get
            {
                Dictionary<Type, Good> result = new Dictionary<Type, Good>();
                foreach (Card card in Cards)
                {
                    foreach (Good good in card.GetGoods())
                    {
                        if (result.ContainsKey(good.GetType()))
                        {
                            result[good.GetType()].Amount += good.Amount;
                        }
                        else
                        {
                            result.Add(good.GetType(), good.Clone());
                        }
                    }
                }
                return result;
            }
        }
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
    }
}
