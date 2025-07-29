using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Factories;
using GameLogic.Events;
using GameLogic.PlayerActions;

namespace GameLogic.Elements.Effects
{
    public class ChooseGood : Effect
    {
        public List<GoodFactory> GoodFactories { get; set; }

        public ChooseGood()
        {
            GoodFactories = new List<GoodFactory>();
        }

        public ChooseGood(ChooseGood chooseGood)
        {
            GoodFactories = chooseGood.GoodFactories;
        }

        public override ChooseGood Clone()
        {
            return new ChooseGood(this);
        }

        public override List<Good> GetGoods()
        {
            return new List<Good>() { m_selectedGood };
        }

        public override void Apply(IGameContext gameContext)
        {
            Player player = gameContext.TurnHandler.CurrentPlayer;
            gameContext.EventManager.Subscribe(GameEventType.TurnStarted, (args) => SelectGood(player, args));
        }

        private void SelectGood(Player player, EventArgs eventArgs)
        {
            if (eventArgs is TurnStarted turnStarted && turnStarted.TurnHandler.CurrentPlayer == player)
            {
                ChooseGoodAction chooseGoodAction = turnStarted.PlayerActionReceiver.ReceivePlayerAction<ChooseGoodAction>(turnStarted.TurnHandler.CurrentPlayer, GoodFactories.Select(goodFactory => new ChooseGoodAction(goodFactory)).ToArray());
                m_selectedGood = chooseGoodAction.GoodFactory.CreateGood();
            }
        }

        private Good? m_selectedGood;
    }
}
