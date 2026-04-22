using GameLogic.Elements.Developments;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using Microsoft.Extensions.DependencyInjection;
using SevenWonders.Common;

namespace GameLogic.Elements
{
    public class GameElements : IGameElements
    {
        public ICardList? Cards => m_cardList;

        public IWonderList? Wonders => m_wonderList;
        public IDevelopmentList? Developments => m_developmentList;

        public GameElements([FromKeyedServices(nameof(MainCardListFactory))] ICardListFactory cardListFactory, IWonderListFactory wonderListFactory, IDevelopmentListFactory developmentListFactory)
        {
            ArgumentChecker.CheckNull(cardListFactory, nameof(cardListFactory));
            ArgumentChecker.CheckNull(wonderListFactory, nameof(wonderListFactory));
            ArgumentChecker.CheckNull(developmentListFactory, nameof(developmentListFactory));

            m_cardListFactory = cardListFactory;
            m_wonderListFactory = wonderListFactory;
            m_developmentListFactory = developmentListFactory;
        }

        public void ResetElements()
        {
            m_cardList = m_cardListFactory.Create();
            m_wonderList = m_wonderListFactory.Create();
            m_developmentList = m_developmentListFactory.Create();
        }

        private IWonderList? m_wonderList;
        private ICardList? m_cardList;
        private IDevelopmentList? m_developmentList;
        private readonly ICardListFactory m_cardListFactory;
        private readonly IWonderListFactory m_wonderListFactory;
        private readonly IDevelopmentListFactory m_developmentListFactory;
    }
}
