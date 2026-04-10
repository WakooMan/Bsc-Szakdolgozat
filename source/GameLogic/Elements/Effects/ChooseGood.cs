using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Factories;
using GameLogic.Events.GameEvents;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using System.Net.Http.Headers;

namespace GameLogic.Elements.Effects
{
    public class ChooseGood : Effect
    {
        public List<GoodFactory> GoodFactories { get; set; }

        public ChooseGood()
        {
            GoodFactories = new List<GoodFactory>();
        }

        private ChooseGood(ChooseGood chooseGood)
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

        public override Task Apply(IGameContext gameContext)
        {
            Player player = gameContext.TurnHandler.CurrentPlayer;
            gameContext.EventManager.Subscribe<TurnStarted>((args) => SelectGood(gameContext, player, args).GetAwaiter().GetResult());
            return Task.CompletedTask;
        }

        private async Task SelectGood(IGameContext gameContext, Player player, TurnStarted eventArgs)
        {
            if (eventArgs.Player == player)
            {
                await gameContext.EventManager.PublishAsync(new OnChooseObjects("Choose Good", GoodFactories.Select(factory => factory.GoodType.Name).ToArray(), false));
                await gameContext.PlayerActionHandler.HandlePlayerActions(gameContext, eventArgs.Player, GoodFactories.Select(goodFactory => {
                    IPlayerAction action = new ChooseGoodAction(goodFactory, SetSelectedGood);
                    return action;
                }).ToList());
                await gameContext.EventManager.PublishAsync(new OnObjectChosen(GoodFactories.Select(factory => factory.GoodType.Name).ToArray(), false));
            }
        }

        private void SetSelectedGood(Good good)
        {
            m_selectedGood = good;
        }

        private Good? m_selectedGood;
    }
}
