using SevenWonders.Game.Logic.Elements.Goods;
using SevenWonders.Game.Logic.Elements.Goods.Factories;
using SevenWonders.Common;

namespace SevenWonders.Game.Logic.PlayerActions
{
    public class ChooseGoodAction : IPlayerAction
    {
        public string Name => m_goodFactory.GoodType.Name;
        public int Id => 8;
        public ChooseGoodAction() { }
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

        public bool DoPlayerAction(IGameContext gameContext)
        {
            m_setter(m_goodFactory.CreateGood());
            return true;
        }

        private readonly GoodFactory m_goodFactory;
        private readonly Action<Good> m_setter;
    }
}
