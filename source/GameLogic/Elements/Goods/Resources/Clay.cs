namespace GameLogic.Elements.Goods.Resources
{
    public class Clay : GameResource
    {
        public Clay(): base() { }

        private Clay(Clay clay):base(clay) { }

        public override Clay Clone()
        {
            return new Clay(this);
        }
    }
}
