using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Factories;
using SevenWonders.Common;

namespace GameLogic.PlayerActions
{
    public class ChooseGoodAction : IPlayerAction
    {
        public string Name => m_goodFactory.CreateGood().GetType().Name;
        public ChooseGoodAction() { }
        public ChooseGoodAction(GoodFactory goodFactory, Action<Good> setter)
        {
            ArgumentChecker.CheckNull(goodFactory, nameof(goodFactory));
            ArgumentChecker.CheckNull(setter, nameof(setter));

            m_goodFactory = goodFactory;
            m_setter = setter;
        }

        public Task<bool> CanPerform(IGameContext gameContext)
        {
            return Task.FromResult(true);
        }

        public Task DoPlayerAction(IGameContext gameContext)
        {
            m_setter(m_goodFactory.CreateGood());
            return Task.CompletedTask;
        }

        private readonly GoodFactory m_goodFactory;
        private readonly Action<Good> m_setter;
    }
}
