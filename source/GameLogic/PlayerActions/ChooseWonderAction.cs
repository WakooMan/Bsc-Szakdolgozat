using GameLogic.Elements;
using GameLogic.Elements.Wonders;

namespace GameLogic.PlayerActions
{
    public class ChooseWonderAction : IPlayerAction
    {
        public ChooseWonderAction(Wonder wonder, Player player)
        {
            m_wonder = wonder;
            m_player = player;
        }

        public bool CanPerform()
        {
            return true;
        }

        public void DoPlayerAction()
        {
            m_player.Wonders.Add(m_wonder);
        }

        private Wonder m_wonder;
        private Player m_player;
    }
}
