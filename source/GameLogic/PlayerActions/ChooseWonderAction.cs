using GameLogic.Elements.Wonders;

namespace GameLogic.PlayerActions
{
    public class ChooseWonderAction : IPlayerAction
    {
        public Wonder Wonder => m_wonder;

        public ChooseWonderAction(Wonder wonder)
        {
            m_wonder = wonder;
        }

        public bool CanPerform()
        {
            return true;
        }

        public void DoPlayerAction()
        {
            throw new NotImplementedException();
        }

        private readonly Wonder m_wonder;
    }
}
