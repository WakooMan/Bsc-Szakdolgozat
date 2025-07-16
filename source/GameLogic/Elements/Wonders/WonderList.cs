namespace GameLogic.Elements.Wonders
{
    public class WonderList : IWonderList
    {
        public List<IWonder> Wonders { get; set; }

        public WonderList()
        {
            Wonders = new List<IWonder>();
        }

        private WonderList(WonderList wonderList)
        {
            Wonders = wonderList.Wonders.Select(wonder => wonder.Clone()).ToList();
        }

        public IWonderList Clone()
        {
            return new WonderList(this);
        }
    }
}
