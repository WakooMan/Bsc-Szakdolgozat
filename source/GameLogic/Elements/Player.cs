using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods;
using GameLogic.Elements.Modifiers;
using GameLogic.Elements.Wonders;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
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
            if (m_eventManager is not null)
            {
                await m_eventManager.PublishAsync(new OnCalculatePlayerProperties(properties));
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
            m_eventManager = null;
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
            m_eventManager = null;
        }

        public void Initialize(IEventManager eventManager)
        {
            m_eventManager = eventManager;
        }

        public bool HasCard(Card card)
        {
            return Cards.Contains(card);
        }

        private int m_money;
        private IEventManager? m_eventManager;
    }
}
