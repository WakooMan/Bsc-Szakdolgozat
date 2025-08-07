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
        private readonly List<IPlayerAction> m_wonderPlayerActions1;
        private readonly List<IPlayerAction> m_wonderPlayerActions2;
        private int m_indexOfPlayer;
        private readonly IGameContext m_gameContext;

        [ImportingConstructor]
        public ChooseWonderHandler(IGameContext gameContext)
        {
            ArgumentChecker.CheckNull(gameContext, nameof(gameContext));
            m_gameContext = gameContext;
            m_wonderPlayerActions1 = new List<IPlayerAction>();
            m_wonderPlayerActions2 = new List<IPlayerAction>();
            m_players = new List<Player>();
            m_wonders = new List<Wonder>();
        }

        public bool WondersChosen => WondersChosenNum == 7;

        public void ChooseWonder()
        {
            ArgumentChecker.CheckPredicateForOperation(() => m_players.Count == 0 || m_wonders.Count == 0, "Wonder cannot be chosen if initialize is not called or all the wonders are chosen!");

            Player player = m_players[m_indexOfPlayer];
            List<IPlayerAction> actions = WondersChosenNum < 4 ? m_wonderPlayerActions1 : m_wonderPlayerActions2;
            IPlayerAction playerAction = m_gameContext.PlayerActionReceiver.ReceivePlayerAction(player, actions);

            if (playerAction.CanPerform(m_gameContext))
            {
                playerAction.DoPlayerAction(m_gameContext);
            }

            actions.Remove(playerAction);
            int nextPlayer = (m_indexOfPlayer == 0) ? 1 : 0;
            m_indexOfPlayer = (WondersChosenNum == 4) ? 1 : nextPlayer;
        }

        public void Initialize(ICollection<Player> players, ICollection<Wonder> wonders)
        {
            ArgumentChecker.CheckPredicateForOperation(() => players.Count != 2 || wonders.Count != 8, "This class should be initialized with exactly 8 wonders and 2 players!");
            
            m_wonderPlayerActions1.Clear();
            m_wonderPlayerActions2.Clear();
            m_players.Clear();
            m_wonders.Clear();
            m_players.AddRange(players);
            m_wonders.AddRange(wonders);
            // m_gameElements.Wonders.Wonders.OrderBy(x => m_randomGenerator.Next()).Take(8)
            // Outer thing, because of multiplayer game
            List<IPlayerAction> playerActions = m_wonders.Select(w => (IPlayerAction)new ChooseWonderAction(w, m_wonders, GetPlayer)).ToList();
            m_wonderPlayerActions1.AddRange(playerActions.Take(4));
            m_wonderPlayerActions1.ForEach(action => playerActions.Remove(action));
            m_wonderPlayerActions2.AddRange(playerActions);
            m_indexOfPlayer = 0;
        }

        private Player GetPlayer() => m_players[m_indexOfPlayer];

        private int WondersChosenNum
        {
            get
            {
                ArgumentChecker.CheckPredicateForOperation(() => m_players is null, "Wonder chosen number cannot be calculated if there are no players set!");
                return m_players.Select(player => player.Wonders.Count).Sum();
            }
        }
    }
}
