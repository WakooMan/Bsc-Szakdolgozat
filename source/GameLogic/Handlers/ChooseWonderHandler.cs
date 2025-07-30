using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using SevenWonders.Common;
using System.ComponentModel.Composition;

namespace GameLogic.Handlers
{
    [Export(typeof(IChooseWonderHandler))]
    public class ChooseWonderHandler: IChooseWonderHandler
    {
        private readonly List<Player> m_players;
        private readonly List<Wonder> m_wonders;
        private readonly Dictionary<Player, List<ChooseWonderAction>> m_wonderPlayerActions1;
        private readonly Dictionary<Player, List<ChooseWonderAction>> m_wonderPlayerActions2;
        private int m_indexOfPlayer;
        private readonly IPlayerActionReceiver m_playerActionReceiver;
        private readonly IRandomGenerator m_randomGenerator;

        [ImportingConstructor]
        public ChooseWonderHandler(IRandomGenerator randomGenerator, IPlayerActionReceiver playerActionReceiver, )
        {
            ArgumentChecker.CheckNull(randomGenerator, nameof(randomGenerator));
            ArgumentChecker.CheckNull(playerActionReceiver, nameof(playerActionReceiver));

            m_randomGenerator = randomGenerator;
            m_playerActionReceiver = playerActionReceiver;
            m_wonderPlayerActions1 = new Dictionary<Player, List<ChooseWonderAction>>();
            m_wonderPlayerActions2 = new Dictionary<Player, List<ChooseWonderAction>>();
            m_players = new List<Player>();
            m_wonders = new List<Wonder>();
        }

        public bool WondersChosen => WondersChosenNum == 7;

        public void ChooseWonder()
        {
            if (m_players.Count == 0 || m_wonders.Count == 0)
            {
                throw new InvalidOperationException("Wonder cannot be chosen if initialize is not called or all the wonders are chosen!");
            }

            Player player = m_players[m_indexOfPlayer];
            List<ChooseWonderAction> actions = WondersChosenNum < 4 ? m_wonderPlayerActions1[player] : m_wonderPlayerActions2[player];
            ChooseWonderAction action = m_playerActionReceiver.ReceivePlayerAction(player, actions);
            action.DoPlayerAction();
            m_wonders.Remove(action.Wonder);
            actions.Remove(action);
            m_indexOfPlayer = (WondersChosenNum == 4) ? 1 : (m_indexOfPlayer == 0) ? 1 : 0;
        }

        public void Initialize(ICollection<Player> players, ICollection<Wonder> wonders)
        {
            m_wonderPlayerActions1.Clear();
            m_wonderPlayerActions2.Clear();
            m_players.Clear();
            m_wonders.Clear();
            m_players.AddRange(players);
            m_wonders.AddRange(wonders);
            // m_gameElements.Wonders.Wonders.OrderBy(x => m_randomGenerator.Next()).Take(8)
            // Outer thing, because of multiplayer game
            foreach (Player player in m_players)
            {
                List<ChooseWonderAction> playerActions = m_wonders.Select(w => new ChooseWonderAction(w, player)).ToList();
                m_wonderPlayerActions1.Add(player, playerActions.Take(4).ToList());
                m_wonderPlayerActions1[player].ForEach(action => playerActions.Remove(action));
                m_wonderPlayerActions2.Add(player, playerActions);
            }
            m_indexOfPlayer = 0;
        }

        private int WondersChosenNum
        {
            get
            {
                if (m_players is null)
                {
                    throw new InvalidOperationException("Wonder chosen number cannot be calculated if there are no players set!");
                }
                return m_players.Select(player => player.Wonders.Count).Sum();
            }
        }
    }
}
