using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods;
using GameLogic.Elements.Modifiers;
using GameLogic.Elements.Wonders;

namespace GameLogic.Elements
{
    public class Player
    {
        public string Name { get; set; }
        public List<Wonder> Wonders { get; }
        public List<Card> Cards { get; }
        public List<Development> Developments { get; }
        public int Money { get; set; }
        public List<Good> Goods { get; }

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
