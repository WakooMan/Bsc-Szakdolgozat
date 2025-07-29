using SevenWonders.Common;

namespace GameLogic.Elements.GameCards
{
    public class MainCardListFactory : ICardListFactory
    {
        public MainCardListFactory(IXmlHandler xmlHandler)
        {
            m_xmlHandler = xmlHandler;
        }

        public ICardList Create()
        {
            return m_xmlHandler.Deserialize<CardList>(CARDLIST_FILE);
        }

        private readonly string CARDLIST_FILE = Path.Combine(Directory.GetCurrentDirectory(), "Data", "AllCards.xml");
        private IXmlHandler m_xmlHandler;
    }
}
