using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using GameLogic.Events.GameEvents;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using SevenWonders.Common;

namespace GameLogic.Handlers
{
    public class ChooseWonderHandler: IChooseWonderHandler
    {
        private readonly List<Player> m_players;
        private readonly List<Wonder> m_wonders;
        private readonly List<ChooseWonderAction> m_wonderPlayerActions1;
        private readonly List<ChooseWonderAction> m_wonderPlayerActions2;
        private int m_indexOfPlayer;
        private IGameContext? m_gameContext;

        public ChooseWonderHandler()
        {
            m_wonderPlayerActions1 = new List<ChooseWonderAction>();
            m_wonderPlayerActions2 = new List<ChooseWonderAction>();
            m_players = new List<Player>();
            m_wonders = new List<Wonder>();
        }

        public bool WondersChosen => WondersChosenNum == 8;

        public async Task ChooseWonder()
        {
            ArgumentChecker.CheckPredicateForOperation(() => m_players.Count == 0 || m_wonders.Count == 0, "Wonder cannot be chosen if initialize is not called or all the wonders are chosen!");
            if (WondersChosenNum == 0)
            {
                await m_gameContext.EventManager.PublishAsync(new OnChooseWonderStateStart(m_wonderPlayerActions1.Select(action => action.Wonder).ToList()));
            }
            if (WondersChosenNum == 4)
            {
                await m_gameContext.EventManager.PublishAsync(new OnFourWondersChosen(m_wonderPlayerActions2.Select(action => action.Wonder).ToList()));
            }

            Player player = m_players[m_indexOfPlayer];
            List<ChooseWonderAction> actions = WondersChosenNum < 4 ? m_wonderPlayerActions1 : m_wonderPlayerActions2;

            IPlayerAction? playerAction = null;

            if (actions.Count > 1)
            {
                await m_gameContext.EventManager.PublishAsync(new ChooseWonderStarted(player));
                (bool completed, playerAction) = await m_gameContext.PlayerActionHandler.HandlePlayerActions(m_gameContext, player, actions.Select(action => (IPlayerAction)action).ToList());
            }
            else
            {
                playerAction = actions.FirstOrDefault();
                if (playerAction is not null)
                {
                    await m_gameContext.PlayerActionHandler.HandlePlayerAction(m_gameContext, player, playerAction);
                }
            }

            if (playerAction is not null)
            {
                actions.Remove((ChooseWonderAction)playerAction);
            }

            int nextPlayer = (m_indexOfPlayer == 0) ? 1 : 0;
            m_indexOfPlayer = (WondersChosenNum == 4) ? 1 : nextPlayer;

            if (WondersChosen)
            {
                await m_gameContext.EventManager.PublishAsync(new OnChooseWonderStateEnd());
            }

        }

        public void Initialize(ICollection<Player> players, ICollection<Wonder> wonders, IGameContext gameContext)
        {
            ArgumentChecker.CheckPredicateForOperation(() => players.Count != 2 || wonders.Count != 8, "This class should be initialized with exactly 8 wonders and 2 players!");
            
            m_gameContext = gameContext;
            m_wonderPlayerActions1.Clear();
            m_wonderPlayerActions2.Clear();
            m_players.Clear();
            m_wonders.Clear();
            m_players.AddRange(players);
            m_wonders.AddRange(wonders);
            List<ChooseWonderAction> playerActions = m_wonders.Select(w => new ChooseWonderAction(w, m_wonders, GetPlayer)).ToList();
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
