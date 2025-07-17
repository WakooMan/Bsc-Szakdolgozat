using System.Xml.Serialization;

namespace GameLogic.Elements.Effects
{
    [XmlInclude(typeof(BuyGoods)),
     XmlInclude(typeof(ChooseGood)),
     XmlInclude(typeof(GetMoney)),
     XmlInclude(typeof(GetMoneyForCard)),
     XmlInclude(typeof(GetMoneyForWonders)),
     XmlInclude(typeof(EnemyLoseMoney)),
     XmlInclude(typeof(BuildFreeFromDroppedCards)),
     XmlInclude(typeof(ChooseDevelopment)),
     XmlInclude(typeof(DropEnemyCard)),
     XmlInclude(typeof(NewTurn)),
     XmlInclude(typeof(VictoryPoints)),
     XmlInclude(typeof(Strength))]
    public abstract class Effect
    {
        public abstract Effect Clone();
        public abstract void Apply();
    }
}
