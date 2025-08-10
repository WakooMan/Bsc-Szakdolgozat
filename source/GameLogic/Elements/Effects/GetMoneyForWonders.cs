using GameLogic.Events;
using GameLogic.GameStates;

namespace GameLogic.Elements.Effects
{
    public class GetMoneyForWonders : Effect
    {
        public int MoneyPerWonder { get; set; }

        public GetMoneyForWonders() { }

        private GetMoneyForWonders(GetMoneyForWonders getMoneyForWonders)
        {
            MoneyPerWonder = getMoneyForWonders.MoneyPerWonder;
        }

        public override GetMoneyForWonders Clone()
        {
            return new GetMoneyForWonders(this);
        }

        public override void Apply(IGameContext gameContext)
        {
            Player player = gameContext.TurnHandler.CurrentPlayer;
            player.Money += MoneyPerWonder * player.Wonders.Count(wonder => wonder.HasBeenBuilt);
        }

    }
}
