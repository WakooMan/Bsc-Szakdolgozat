using GameLogic;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;
using GameLogic.Interfaces;
using SevenWonders.Common;

namespace SevenWonders.AI.Model.Services
{
    public class PlayerActionMaskReceiver : IPlayerActionMaskReceiver
    {
        public PlayerActionMaskReceiver(IGame game)
        {
            m_game = game;
            m_nodes = new ICardNode?[20];
        }

        public void Initialize()
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
            for (int i = allCards.Count; i < m_nodes.Length; i++)
            {
                m_nodes[i] = null;
            }
        }

        public List<int> ReceivePlayerActionMask(PhaseIndicator phaseIndicator, PlayerActionWrapper[] playerActions)
        {
            GameLog.Info($"ReceivePlayerActionMask: Phase={phaseIndicator}, ActionCount={playerActions.Length}");
            int[] actionMask = new int[23];
            ICardComposition cardComposition = m_game.Context.AgeHandler.CurrentAge.Composition;
            IReadOnlyList<ICardNode> availableCards = cardComposition.AvailableCards;
            for (int i = 0; i < 20; i++)
            {
                if (m_nodes[i] is not null && !cardComposition.AllCards.Contains(m_nodes[i]))
                {
                    m_nodes[i] = null;
                }

                bool isAvailable = m_nodes[i] is not null && availableCards.Contains(m_nodes[i]);
                actionMask[i] = phaseIndicator == PhaseIndicator.ChooseCard && isAvailable ? 1 : 0;
            }

            for (int i = 20; i < 23; i++)
            {
                int playerActionMask = 0;
                foreach (var playerAction in playerActions)
                {
                    if (playerAction.PlayerAction.Id == i)
                    {
                        playerActionMask = playerAction.CanPerform ? 1 : 0;
                    }
                }
                actionMask[i] = phaseIndicator == PhaseIndicator.ChooseAction ? playerActionMask : 0;
            }
            GameLog.Info($"Action mask: [{string.Join(",", actionMask)}]");
            return actionMask.ToList();
        }

        public ICardNode? GetNode(int index)
        {
            if (index < 0 || index >= m_nodes.Length)
            {
                return null;
            }
            return m_nodes[index];
        }

        public List<int> ReceiveEmptyPlayerActionMask()
        {
            return new int[23].ToList();
        }

        private readonly IGame m_game;
        private readonly ICardNode?[] m_nodes;
    }
}
