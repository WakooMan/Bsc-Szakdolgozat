namespace GameLogic.Elements.Wonders
{
    public class Wonder: IWonder
    {
        public string Name { get; set; }

        public IWonder Clone()
        {
            throw new NotImplementedException();
        }
    }
}
