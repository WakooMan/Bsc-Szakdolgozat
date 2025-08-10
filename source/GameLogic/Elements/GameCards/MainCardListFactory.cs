using SevenWonders.Common;
using System.ComponentModel.Composition;

namespace GameLogic.Elements.GameCards
{
    [Export(nameof(MainCardListFactory), typeof(ICardListFactory))]
    public class MainCardListFactory : ICardListFactory
    {
        [ImportingConstructor]
        public MainCardListFactory(IXmlHandler xmlHandler)
        {
            m_xmlHandler = xmlHandler;
        }

        public ICardList Create()
        {
            return m_xmlHandler.Deserialize<CardList>(CARDLIST_FILE);
        }

        private readonly string CARDLIST_FILE = Path.Combine(Directory.GetCurrentDirectory(), "Data", "AllCards.xml");
        private readonly IXmlHandler m_xmlHandler;
    }
}
