namespace GameLogic.Elements.Wonders
{
    public class WonderList : IWonderList
    {
        public List<Wonder> Wonders { get; set; }

        public WonderList()
        {
            Wonders = new List<Wonder>();
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
