using SevenWonders.Common;
using System.ComponentModel.Composition;

namespace GameLogic.Elements.Developments
{
    [Export(typeof(IDevelopmentListFactory))]
    public class DevelopmentListFactory : IDevelopmentListFactory
    {
        [ImportingConstructor]
        public DevelopmentListFactory(IXmlHandler xmlHandler)
        {
            m_xmlHandler = xmlHandler;
        }

        public IDevelopmentList Create()
        {
            return m_xmlHandler.Deserialize<DevelopmentList>(CARDLIST_FILE);
        }

        private readonly string CARDLIST_FILE = Path.Combine(Directory.GetCurrentDirectory(), "Data", "AllDevelopments.xml");
        private readonly IXmlHandler m_xmlHandler;
    }
}
