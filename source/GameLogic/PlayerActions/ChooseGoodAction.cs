using GameLogic.Elements.Goods.Factories;

namespace GameLogic.PlayerActions
{
    public class ChooseGoodAction : IPlayerAction
    {
        public GoodFactory GoodFactory { get; private set; }

        public ChooseGoodAction(GoodFactory goodFactory)
        {
            GoodFactory = goodFactory;
        }

        public bool CanPerform()
        {
            return true;
        }

        public void DoPlayerAction()
        {
            throw new NotImplementedException("This action does not need implementation, because only the containing data is needed!");
        }
    }
}
