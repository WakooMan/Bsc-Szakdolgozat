using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using SevenWonders.Common;

namespace GameLogic.Handlers
{
    public class ChooseWonderHandler: IChooseWonderHandler
    {
        private List<Player>? m_players;
        private readonly List<Wonder> m_wonders;
        private readonly Dictionary<Player, List<IPlayerAction>> m_wonderPlayerActions1;
        private readonly Dictionary<Player, List<IPlayerAction>> m_wonderPlayerActions2;
        private int m_indexOfPlayer;
        private readonly IPlayerActionReceiver m_playerActionReceiver;

        public ChooseWonderHandler(IRandomGenerator randomGenerator, IPlayerActionReceiver playerActionReceiver, IWonderList wonderList)
        {
            m_players = null;
            m_wonderPlayerActions1 = new Dictionary<Player, List<IPlayerAction>>();
            m_wonderPlayerActions2 = new Dictionary<Player, List<IPlayerAction>>();
            m_wonders = wonderList.Wonders.OrderBy(x => randomGenerator.Next()).Take(8).ToList();
            foreach (Player player in m_players)
            {
                List<IPlayerAction> playerActions = m_wonders.Select(w => (IPlayerAction)new ChooseWonderAction(w, player)).ToList();
                m_wonderPlayerActions1.Add(player, playerActions.Take(4).ToList());
                m_wonderPlayerActions1[player].ForEach(action => playerActions.Remove(action));
                m_wonderPlayerActions2.Add(player, playerActions);
            }
            m_indexOfPlayer = 0;
            m_playerActionReceiver = playerActionReceiver;
        }

        public bool WondersChosen => WondersChosenNum == 7;

        public void ChooseWonder()
        {
            if (m_players is null)
            {
                throw new InvalidOperationException("Wonder cannot be chosen if there are no players set!");
            }
            Player player = m_players[m_indexOfPlayer];
            m_playerActionReceiver.ReceivePlayerAction(player, WondersChosenNum < 4 ? m_wonderPlayerActions1[player] : m_wonderPlayerActions2[player]).DoPlayerAction();

            m_indexOfPlayer = (WondersChosenNum == 4) ? 1 : (m_indexOfPlayer == 0) ? 1 : 0;
        }

        public void SetPlayers(ICollection<Player> players)
        {
            m_players = [.. players];
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
