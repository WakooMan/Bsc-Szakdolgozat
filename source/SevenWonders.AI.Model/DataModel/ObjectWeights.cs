namespace SevenWonders.AI.Model.DataModel
{
    public class ObjectWeights
    {
        public List<ObjectWeight> WonderWeights { get; set; }
        public List<ObjectWeight> CardWeights { get; set; }
        public List<ObjectWeight> DevelopmentWeights { get; set; }
        public ObjectWeights()
        {
            WonderWeights = new List<ObjectWeight>();
            CardWeights = new List<ObjectWeight>();
            DevelopmentWeights = new List<ObjectWeight>();
        }
    }
}
