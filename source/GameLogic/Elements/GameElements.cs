using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;

namespace GameLogic.Elements
{
    public class GameElements : IGameElements
    {
        public ICardList Cards => m_cardList.Clone();

        public IWonderList Wonders => m_wonderList.Clone();

        public GameElements(ICardListFactory cardListFactory, IWonderListFactory wonderListFactory)
        {
            m_cardList = cardListFactory.Create();
            m_wonderList = wonderListFactory.Create();
        }

        private readonly IWonderList m_wonderList;
        private readonly ICardList m_cardList;
    }
}
