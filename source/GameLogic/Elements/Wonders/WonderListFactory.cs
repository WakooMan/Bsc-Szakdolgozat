using SevenWonders.Common;
using System.ComponentModel.Composition;

namespace GameLogic.Elements.Wonders
{
    [Export(typeof(IWonderListFactory))]
    public class WonderListFactory: IWonderListFactory
    {
        [ImportingConstructor]
        public WonderListFactory(IXmlHandler xmlHandler)
        {
            ArgumentChecker.CheckNull(xmlHandler, nameof(xmlHandler));

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
