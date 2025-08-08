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

        public ChooseDevelopmentAction(Player player, Development development)
        {
            ArgumentChecker.CheckNull(player, nameof(player));
            ArgumentChecker.CheckNull(development, nameof(development));

            m_player = player;
            m_development = development;
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return !m_player.Developments.Contains(m_development);
        }

        public void DoPlayerAction(IGameContext gameContext)
        {
            ArgumentChecker.CheckPredicateForOperation(() => m_player.Developments.Contains(m_development), "Cannot perform action, because player already has the development!");

           m_player.Developments.Add(m_development);
           m_development.OnDevelopmentEstablished(gameContext);
        }

        private readonly Development m_development;
        private readonly Player m_player;
    }
}
