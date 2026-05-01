using SevenWonders.Common;

namespace SevenWonders.Game.Logic.Elements.Wonders
{
    public class WonderListFactory: IWonderListFactory
    {
        public WonderListFactory(IXmlHandler xmlHandler)
        {
            ArgumentChecker.CheckNull(xmlHandler, nameof(xmlHandler));

            m_xmlHandler = xmlHandler;
        }

        public IWonderList Create()
        {
            return m_xmlHandler.DeserializeEmbeddedResource<WonderList>(CARDLIST_FILE);
        }

        private readonly string CARDLIST_FILE = "SevenWonders.Game.Logic.Data.AllWonders.xml";
        private readonly IXmlHandler m_xmlHandler;
    }
}
