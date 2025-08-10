using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Factories;
using SevenWonders.Common;

namespace GameLogic.PlayerActions
{
    public class ChooseGoodAction : IPlayerAction
    {
        public ChooseGoodAction(GoodFactory goodFactory, Action<Good> setter)
        {
            ArgumentChecker.CheckNull(goodFactory, nameof(goodFactory));
            ArgumentChecker.CheckNull(setter, nameof(setter));

            m_goodFactory = goodFactory;
            m_setter = setter;
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return true;
        }

        public void DoPlayerAction(IGameContext gameContext)
        {
            m_setter(m_goodFactory.CreateGood());
        }

        private readonly GoodFactory m_goodFactory;
        private readonly Action<Good> m_setter;
    }
}
