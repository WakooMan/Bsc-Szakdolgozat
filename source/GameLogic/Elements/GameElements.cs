using GameLogic.Elements.Developments;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using SevenWonders.Common;
using System.ComponentModel.Composition;

namespace GameLogic.Elements
{
    [Export(typeof(IGameElements))]
    public class GameElements : IGameElements
    {
        public ICardList Cards => m_cardList.Clone();

        public IWonderList Wonders => m_wonderList.Clone();
        public IDevelopmentList Developments => m_developmentList.Clone();

        [ImportingConstructor]
        public GameElements([Import(nameof(MainCardListFactory), typeof(ICardListFactory))] ICardListFactory cardListFactory, IWonderListFactory wonderListFactory, IDevelopmentListFactory developmentListFactory)
        {
            ArgumentChecker.CheckNull(cardListFactory, nameof(cardListFactory));
            ArgumentChecker.CheckNull(wonderListFactory, nameof(wonderListFactory));
            ArgumentChecker.CheckNull(developmentListFactory, nameof(developmentListFactory));

            m_cardList = cardListFactory.Create();
            m_wonderList = wonderListFactory.Create();
            m_developmentList = developmentListFactory.Create();
        }

        private readonly IWonderList m_wonderList;
        private readonly ICardList m_cardList;
        private readonly IDevelopmentList m_developmentList;
    }
}
