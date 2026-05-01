using SevenWonders.Common;

namespace SevenWonders.Game.Logic.Elements.Military
{
    public class MilitaryBoardFactory : IMilitaryBoardFactory
    {
        public MilitaryBoardFactory(IXmlHandler xmlHandler)
        {
            m_xmlHandler = xmlHandler;
        }

        public IMilitaryBoard Create()
        {
            return m_xmlHandler.DeserializeEmbeddedResource<MilitaryBoard>(CARDLIST_FILE);
        }

        private readonly string CARDLIST_FILE = "GameLogic.Data.MilitaryBoard.xml";
        private readonly IXmlHandler m_xmlHandler;
    }
}
