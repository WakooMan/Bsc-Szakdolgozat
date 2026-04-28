using SevenWonders.AI.Model.DataModel;
using SevenWonders.Common;

namespace SevenWonders.AI.Model
{
    public class WeightConfiguration : IWeightConfiguration
    {
        public ObjectWeights ObjectWeights { get; }

        public WeightConfiguration(IXmlHandler xmlHandler)
        {
            m_xmlHandler = xmlHandler;
            ObjectWeights = m_xmlHandler.DeserializeEmbeddedResource<ObjectWeights>(WEIGHTS_FILE);

        }

        private readonly string WEIGHTS_FILE = "SevenWonders.AI.Model.Data.AllWeights.xml";
        private readonly IXmlHandler m_xmlHandler;
    }
}
