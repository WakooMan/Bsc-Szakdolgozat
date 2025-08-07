using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Factories;

namespace GameLogic.PlayerActions
{
    public class ChooseGoodAction : IPlayerAction
    {
        public ChooseGoodAction(GoodFactory goodFactory, Action<Good> setter)
        {
            m_goodFactory = goodFactory;
            m_setter = setter;
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return m_goodFactory is not null && m_setter is not null;
        }

        public void DoPlayerAction(IGameContext gameContext)
        {
            m_setter(m_goodFactory.CreateGood());
        }

        private readonly GoodFactory m_goodFactory;
        private readonly Action<Good> m_setter;
    }
}
