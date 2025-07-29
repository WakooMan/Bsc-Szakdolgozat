using GameLogic.Elements.Goods;
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
     XmlInclude(typeof(Strength)),
     XmlInclude(typeof(Mathematics)),
     XmlInclude(typeof(MoneyOnChainBuild)),
     XmlInclude(typeof(PlusStrengthOnRedCardBuild)),
     XmlInclude(typeof(CheaperBuilding)),
     XmlInclude(typeof(Law)),
     XmlInclude(typeof(Economics)),
     XmlInclude(typeof(Teology))]
    public abstract class Effect
    {
        public abstract Effect Clone();
        public virtual void Apply(IGameContext gameContext) { }
        public virtual List<Good> GetGoods()
        {
            return new List<Good>();
        }
    }
}
