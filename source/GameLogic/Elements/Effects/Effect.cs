using GameLogic.Elements.Goods;
using GameLogic.Events.GameEvents;
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
        public virtual Task Apply(IGameContext gameContext, int playerId) { return Task.CompletedTask; }
        public virtual Task Unapply(IGameContext gameContext, int playerId) { return Task.CompletedTask; }

        public virtual Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            playerProperties.AddEffect(this);
            return Task.CompletedTask;
        }

        public virtual Task OnBeforeGameEnded(Player owner, Player opponent)
        {
            return Task.CompletedTask;
        }
    }
}
