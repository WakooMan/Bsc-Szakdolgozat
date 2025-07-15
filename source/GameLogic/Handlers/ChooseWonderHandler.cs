using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;

namespace GameLogic.Handlers
{
    public class ChooseWonderHandler: IChooseWonderHandler
    {
        private List<Player> m_players;
        private readonly List<IPlayerAction> m_wonderPlayerActions1;
        private readonly List<IPlayerAction> m_wonderPlayerActions2;
        private int m_indexOfPlayer;
        private readonly IPlayerActionReceiver m_playerActionReceiver;

        public ChooseWonderHandler(IPlayerActionReceiver playerActionReceiver, IWonderList wonderList, ICollection<Player> players)
        {
            Random random = new Random();
            m_players = new List<Player>(players);
            List<IPlayerAction> playerActions = wonderList.Wonders.Select(w => (IPlayerAction)new ChooseWonderAction(w)).ToList();
            m_wonderPlayerActions1 = playerActions.OrderBy(x => random.Next()).Take(4).ToList();
            m_wonderPlayerActions1.ForEach(action => playerActions.Remove(action));
            m_wonderPlayerActions2 = playerActions.OrderBy(x => random.Next()).Take(4).ToList();
            m_indexOfPlayer = 0;
            m_playerActionReceiver = playerActionReceiver;
        }

        public bool WondersChosen => WondersChosenNum == 7;

        public ICollection<Player> Players => m_players;

        public void ChooseWonder()
        {
            Player player = m_players[m_indexOfPlayer];
            m_playerActionReceiver.ReceivePlayerAction(player, WondersChosenNum < 4 ? m_wonderPlayerActions1 : m_wonderPlayerActions2);

            m_indexOfPlayer = (WondersChosenNum == 4) ? 1 : (m_indexOfPlayer == 0) ? 1 : 0;
        }

        private int WondersChosenNum => Players.Select(player => player.Wonders.Count).Sum();
    }
}
