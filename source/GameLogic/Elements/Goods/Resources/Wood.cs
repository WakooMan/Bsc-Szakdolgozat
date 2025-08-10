namespace GameLogic.Elements.Goods.Resources
{
    public class Wood : GameResource
    {
        public Wood(): base() { }

        private Wood(Wood wood) : base(wood) { }

        public override Wood Clone()
        {
            return new Wood(this);
        }
    }
}
