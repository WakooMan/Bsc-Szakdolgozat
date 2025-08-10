using GameLogic.Elements;
using GameLogic.Elements.Modifiers;
using SevenWonders.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.PlayerActions
{
    public class ChooseDevelopmentAction : IPlayerAction
    {

        public ChooseDevelopmentAction(Player player, Development development, List<Development> developments)
        {
            ArgumentChecker.CheckNull(player, nameof(player));
            ArgumentChecker.CheckNull(development, nameof(development));
            ArgumentChecker.CheckNull(developments, nameof(developments));

            m_player = player;
            m_development = development;
            m_developments = developments;
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return !m_player.Developments.Contains(m_development) && m_developments.Contains(m_development);
        }

        public void DoPlayerAction(IGameContext gameContext)
        {
            ArgumentChecker.CheckPredicateForOperation(() => m_player.Developments.Contains(m_development), "Cannot perform action, because player already has the development!");
            ArgumentChecker.CheckPredicateForOperation(() => !m_developments.Contains(m_development), "Cannot perform action, because development list does not contain the development!");

            m_player.Developments.Add(m_development);
           m_development.OnDevelopmentEstablished(gameContext);
           m_developments.Remove(m_development);
        }

        private readonly List<Development> m_developments;
        private readonly Development m_development;
        private readonly Player m_player;
    }
}
