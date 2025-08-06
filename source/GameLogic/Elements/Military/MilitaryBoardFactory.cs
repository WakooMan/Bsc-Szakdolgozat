using SevenWonders.Common;
using System.ComponentModel.Composition;

namespace GameLogic.Elements.Military
{
    [Export(typeof(IMilitaryBoardFactory))]
    public class MilitaryBoardFactory : IMilitaryBoardFactory
    {
        [ImportingConstructor]
        public MilitaryBoardFactory(IXmlHandler xmlHandler)
        {
            m_xmlHandler = xmlHandler;
        }

        public IMilitaryBoard Create()
        {
            return m_xmlHandler.Deserialize<MilitaryBoard>(CARDLIST_FILE);
        }

        private readonly string CARDLIST_FILE = Path.Combine(Directory.GetCurrentDirectory(), "Data", "MilitaryBoard.xml");
        private readonly IXmlHandler m_xmlHandler;
    }
}
