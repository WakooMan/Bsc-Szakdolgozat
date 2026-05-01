using SevenWonders.Common;

namespace SevenWonders.Game.Logic.Elements.Developments
{
    public class DevelopmentListFactory : IDevelopmentListFactory
    {
        public DevelopmentListFactory(IXmlHandler xmlHandler)
        {
            m_xmlHandler = xmlHandler;
        }

        public IDevelopmentList Create()
        {
            return m_xmlHandler.DeserializeEmbeddedResource<DevelopmentList>(CARDLIST_FILE);
        }

        private readonly string CARDLIST_FILE = "GameLogic.Data.AllDevelopments.xml";
        private readonly IXmlHandler m_xmlHandler;
    }
}
