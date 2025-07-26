using GameLogic.GameStates;

namespace GameLogic.Elements.Effects
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

    }
}
