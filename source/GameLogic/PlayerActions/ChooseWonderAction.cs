using GameLogic.Elements;
using GameLogic.Elements.Wonders;

namespace GameLogic.PlayerActions
{
    public class ChooseWonderAction : IPlayerAction
    {
        public Wonder Wonder => m_wonder;

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

        private readonly Wonder m_wonder;
        private readonly Player m_player;
    }
}
