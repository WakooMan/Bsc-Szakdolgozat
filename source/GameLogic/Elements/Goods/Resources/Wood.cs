namespace GameLogic.Elements.Goods.Resources
{
    public class Wood : GameResource
    {
        public override GameResource Clone()
        {
            return new Wood();
        }
    }
}
