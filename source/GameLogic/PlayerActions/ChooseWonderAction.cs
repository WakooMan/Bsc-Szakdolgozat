using GameLogic.Elements;
using GameLogic.Elements.Wonders;

namespace GameLogic.PlayerActions
{
    public class ChooseWonderAction : IPlayerAction
    {
        private IWonder m_wonder;

        public ChooseWonderAction(IWonder wonder)
        {
            m_wonder = wonder;
        }
        public void DoPlayerAction(Player player)
        {
            player.Wonders.Add(m_wonder);
        }
    }
}
