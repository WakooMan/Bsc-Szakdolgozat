namespace GameLogic.Elements.Wonders
{
    public class WonderList : IWonderList
    {
        public List<IWonder> Wonders { get; set; }

        public IWonderList Clone()
        {
            throw new NotImplementedException();
        }

        public WonderList()
        {
            Wonders = new List<IWonder>();
        }
    }
}
