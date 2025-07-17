using SevenWonders.Common;

namespace GameLogic.Elements.Wonders
{
    public class WonderListFactory: IWonderListFactory
    {
        public WonderListFactory(IXmlHandler xmlHandler)
        {
            m_xmlHandler = xmlHandler;
        }

        public IWonderList Create()
        {
            return m_xmlHandler.Deserialize<WonderList>(CARDLIST_FILE);
        }

        private readonly string CARDLIST_FILE = Path.Combine(Directory.GetCurrentDirectory(), "Data", "AllWonders.xml");
        private IXmlHandler m_xmlHandler;
    }
}
