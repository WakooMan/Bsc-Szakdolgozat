using SevenWonders.Common;

namespace SevenWonders.Game.Logic.Elements.GameCards
{
    public class MainCardListFactory : ICardListFactory
    {
        public MainCardListFactory(IXmlHandler xmlHandler)
        {
            m_xmlHandler = xmlHandler;
        }

        public ICardList Create()
        {
            return m_xmlHandler.DeserializeEmbeddedResource<CardList>(CARDLIST_FILE);
        }

        private readonly string CARDLIST_FILE = "GameLogic.Data.AllCards.xml";
        private readonly IXmlHandler m_xmlHandler;
    }
}
