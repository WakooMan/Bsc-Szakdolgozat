using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Factories;
using GameLogic.Events.GameEvents;
using GameLogic.Interfaces;
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
            if (m_selectedGood is null)
            {
                return base.GetGoods();
            }

            return new List<Good>() { m_selectedGood };
        }

        public override void Apply(IGameContext gameContext)
        {
            Player player = gameContext.TurnHandler.CurrentPlayer;
            gameContext.EventManager.Subscribe<TurnStarted>((args) => SelectGood(gameContext, player, args));
        }

        private void SelectGood(IGameContext gameContext, Player player, TurnStarted eventArgs)
        {
            if (eventArgs.Player == player)
            {
                IPlayerAction playerAction = gameContext.PlayerActionReceiver.ReceivePlayerAction(eventArgs.Player, GoodFactories.Select(goodFactory => new ChooseGoodAction(goodFactory, SetSelectedGood)).ToArray());
                
                if (playerAction.CanPerform(gameContext))
                {
                    playerAction.DoPlayerAction(gameContext);
                }
            }
        }

        private void SetSelectedGood(Good good)
        {
            m_selectedGood = good;
        }

        private Good? m_selectedGood;
    }
}
