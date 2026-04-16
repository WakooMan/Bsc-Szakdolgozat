using GameLogic.Elements.GameCards;
using GameLogic.Elements.Modifiers;
using GameLogic.Elements.Wonders;
using GameLogic.GameStructures;
using GameLogic.Interfaces;
using System.Xml.Serialization;

namespace GameLogic.Elements
{
    public class Player
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<Wonder> Wonders { get; set; }
        public List<Card> Cards { get; set; }
        public List<Development> Developments { get; set; }
        public IPlayerActionReceiver? PlayerActionReceiver { get; set; }

        [XmlIgnore]
        public ICardNode? PickedCard { get; set; }
        public int Money
        {
            get
            {
                return m_money;
            }
            set
            {
                m_money = (value < 0) ? 0 : value;
            }
        }
        public async Task<PlayerProperties> GetPlayerProperties()
        {
            PlayerProperties properties = new PlayerProperties(this);

            foreach (Card card in Cards)
            {
                await card.OnCalculatePlayerProperties(properties);
            }

            foreach (Wonder wonder in Wonders)
            {
                await wonder.OnCalculatePlayerProperties(properties);
            }

            foreach (Development development in Developments)
            {
                await development.OnCalculatePlayerProperties(properties);
            }

            return properties;
        }

        public Player()
        {
            Name = "";
            Id = 0;
            Wonders = new List<Wonder>();
            Cards = new List<Card>();
            Developments = new List<Development>();
            Money = 0;
        }

        public Player(IPlayerActionReceiver playerActionReceiver, string name, int id, int money)
        {
            PlayerActionReceiver = playerActionReceiver;
            Name = name;
            Id = id;
            Wonders = new List<Wonder>();
            Cards = new List<Card>();
            Developments = new List<Development>();
            Money = money;
        }

        public async Task OnBeforeGameEnded(Player opponent)
        {
            foreach (Card card in Cards)
            {
                await card.OnBeforeGameEnded(this, opponent);
            }

            foreach (Wonder wonder in Wonders)
            {
                await wonder.OnBeforeGameEnded(this, opponent);
            }

            foreach (Development development in Developments)
            {
                await development.OnBeforeGameEnded(this, opponent);
            }
        }

        private int m_money;
    }
}
