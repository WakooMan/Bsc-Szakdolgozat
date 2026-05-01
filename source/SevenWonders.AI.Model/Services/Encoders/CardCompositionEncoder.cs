using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.GameStructures;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public class CardCompositionEncoder : ICardCompositionEncoder
    {
        public CardCompositionEncoder(IGame game, ICardNodeEncoder cardNodeEncoder)
        {
            m_game = game;
            m_nodes = new ICardNode?[20];
            m_cardNodeEncoder = cardNodeEncoder;
            m_initialized = false;
        }

        public void InitializeComposition()
        {
            m_game.Context.EventManager.Subscribe<OnAgeStarted>(AgeStarted);
        }

        private void AgeStarted(OnAgeStarted started)
        {
            ICardComposition cardComposition = started.Age.Composition;
            IReadOnlyList<ICardNode> allCards = cardComposition.AllCards;
            for (int i = 0; i < allCards.Count && i < m_nodes.Length; i++)
            {
                m_nodes[i] = allCards[i];
            }
            m_initialized = true;
        }

        public void ClearComposition()
        {
            for (int i = 0; i < m_nodes.Length; i++)
            {
                m_nodes[i] = null;
            }
            m_initialized = false;
        }


        public void EncodeNodes(List<float> vector, PlayerProperties ownerProperties, PlayerProperties opponentProperties)
        {
            if (!m_initialized)
            {
                throw new InvalidOperationException("CardCompositionEncoder must be initialized before encoding nodes.");
            }

            ICardComposition cardComposition = m_game.Context.AgeHandler.CurrentAge.Composition;
            for (int i = 0; i < 20; i++)
            {
                if (m_nodes[i] is not null && !cardComposition.AllCards.Contains(m_nodes[i]))
                {
                    m_nodes[i] = null;
                }

                ICardNode? cardNode = m_nodes[i];
                if (cardNode is not null)
                {
                    m_cardNodeEncoder.EncodeCardNode(vector, cardNode, cardComposition.AvailableCards.Contains(cardNode), ownerProperties, opponentProperties);
                }
                else
                {
                    m_cardNodeEncoder.EncodeEmptyCardNode(vector);
                }
            }
        }

        private readonly IGame m_game;
        private readonly ICardNodeEncoder m_cardNodeEncoder;
        private readonly ICardNode?[] m_nodes;
        private bool m_initialized;
    }
}
