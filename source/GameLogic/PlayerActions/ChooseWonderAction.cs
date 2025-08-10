using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using SevenWonders.Common;
using System;

namespace GameLogic.PlayerActions
{
    public class ChooseWonderAction : IPlayerAction
    {
        public Wonder Wonder => m_wonder;

        public ChooseWonderAction(Wonder wonder, List<Wonder> wonders, Func<Player> player)
        {
            ArgumentChecker.CheckNull(wonder, nameof(wonder));
            ArgumentChecker.CheckNull(wonders, nameof(wonders));
            ArgumentChecker.CheckNull(player, nameof(player));

            m_wonder = wonder;
            m_wonders = wonders;
            m_player = player;
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return m_wonders.Contains(m_wonder);
        }

        public void DoPlayerAction(IGameContext gameContext)
        {
            ArgumentChecker.CheckPredicateForOperation(() => !m_wonders.Contains(m_wonder), "Wonder list does not contain the wonder! Action cannot be performed!");

            m_player().Wonders.Add(m_wonder);
            m_wonders.Remove(m_wonder);
        }

        private readonly Wonder m_wonder;
        private readonly Func<Player> m_player;
        private readonly List<Wonder> m_wonders;
    }
}
