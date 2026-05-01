using SevenWonders.AI.Model.DataModel;
using SevenWonders.Common;

namespace SevenWonders.AI.Trainer.Server.DataModel
{
    public class EnemyChanceConfiguration: IEnemyChanceConfiguration
    {
        public EnemyChances Chances { get; }

        public EnemyChanceConfiguration(IXmlHandler xmlHandler)
        {
            m_xmlHandler = xmlHandler;
            Chances = m_xmlHandler.DeserializeEmbeddedResource<EnemyChances>(CHANCES_FILE);

        }

        private readonly string CHANCES_FILE = "SevenWonders.AITrainerServer.Data.EnemyChances.xml";
        private readonly IXmlHandler m_xmlHandler;
    }
}
