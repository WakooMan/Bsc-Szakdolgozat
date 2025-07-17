using GameLogic.Elements;
using GameLogic.Elements.Wonders;

namespace GameLogic.PlayerActions
{
    public class ChooseWonderAction : IPlayerAction
    {
        private Wonder m_wonder;

        public ChooseWonderAction(Wonder wonder)
        {
            m_wonder = wonder;
        }
        public void DoPlayerAction(Player player)
        {
            player.Wonders.Add(m_wonder);
        }
    }
}
