using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Military;
using SevenWonders.Game.Logic.Elements.Modifiers;
using SevenWonders.Game.Logic.Elements.Wonders;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.GameStructures;
using SevenWonders.Game.Logic.Interfaces;
using System.Xml.Serialization;

namespace SevenWonders.Game.Logic.Elements
{
    public class Player
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<Wonder> Wonders { get; set; }
        public List<Card> Cards { get; set; }
        public List<Development> Developments { get; set; }
        public List<MilitaryCard> MilitaryCards { get; set; }
        public IPlayerActionReceiver? PlayerActionReceiver { get; set; }
        public event Func<Player, OnCardBuilt, Task>? OnCardBuilt;
        public event Func<Player, OnWonderBuilt, Task>? OnWonderBuilt;

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
        public PlayerProperties GetPlayerProperties(Player opponent)
        {
            PlayerProperties properties = new PlayerProperties(this, opponent);

            foreach (Card card in Cards)
            {
                card.OnCalculatePlayerProperties(properties);
            }

            foreach (Wonder wonder in Wonders)
            {
                if (wonder.HasBeenBuilt)
                {
                    wonder.OnCalculatePlayerProperties(properties);
                }
            }

            foreach (Development development in Developments)
            {
                development.OnCalculatePlayerProperties(properties);
            }

            foreach (MilitaryCard militaryCard in MilitaryCards)
            {
                militaryCard.OnCalculatePlayerProperties(properties);
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
            MilitaryCards = new List<MilitaryCard>();
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
            MilitaryCards = new List<MilitaryCard>();
            Money = money;
        }

        public void OnBuildWonder(OnWonderBuilt onWonderBuilt)
        {
            OnWonderBuilt?.Invoke(this, onWonderBuilt);
        }

        public void OnBuildCard(OnCardBuilt onCardBuilt)
        {
            OnCardBuilt?.Invoke(this, onCardBuilt);
        }

        private int m_money;
    }
}
