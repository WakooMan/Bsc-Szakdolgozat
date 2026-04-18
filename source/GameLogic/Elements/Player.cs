using GameLogic.Elements.GameCards;
using GameLogic.Elements.Military;
using GameLogic.Elements.Modifiers;
using GameLogic.Elements.Wonders;
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
        public async Task<PlayerProperties> GetPlayerProperties(Player opponent)
        {
            PlayerProperties properties = new PlayerProperties(this, opponent);

            foreach (Card card in Cards)
            {
                await card.OnCalculatePlayerProperties(properties);
            }

            foreach (Wonder wonder in Wonders)
            {
                if (wonder.HasBeenBuilt)
                {
                    await wonder.OnCalculatePlayerProperties(properties);
                }
            }

            foreach (Development development in Developments)
            {
                await development.OnCalculatePlayerProperties(properties);
            }

            foreach (MilitaryCard militaryCard in MilitaryCards)
            {
                await militaryCard.OnCalculatePlayerProperties(properties);
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

        public async Task OnBuildWonder(OnWonderBuilt onWonderBuilt)
        {
            await (OnWonderBuilt?.Invoke(this, onWonderBuilt) ?? Task.CompletedTask);
        }

        public async Task OnBuildCard(OnCardBuilt onCardBuilt)
        {
            await (OnCardBuilt?.Invoke(this, onCardBuilt) ?? Task.CompletedTask);
        }

        private int m_money;
    }
}
