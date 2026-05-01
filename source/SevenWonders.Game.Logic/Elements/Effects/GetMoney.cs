using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.GameStates;

namespace SevenWonders.Game.Logic.Elements.Effects
{
    public class GetMoney : Effect
    {
        public int Money { get; set; }

        public GetMoney() { }

        private GetMoney(GetMoney getMoney)
        {
            Money = getMoney.Money;
        }

        public override GetMoney Clone()
        {
            return new GetMoney(this);
        }

        public override void Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.Money += Money;
        }

    }
}
