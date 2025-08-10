using GameLogic;
using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.Handlers;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic_UnitTests.Elements.Effects
{
    public class BuyGoodsTests
    {
        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_turnHandler = Substitute.For<ITurnHandler>();
            m_eventManager = Substitute.For<IEventManager>();
            m_player = new Player();
            m_gameContext.TurnHandler.Returns(m_turnHandler);
            m_gameContext.EventManager.Returns(m_eventManager);
            m_turnHandler.CurrentPlayer.Returns(m_player);
            m_buyGoods = new BuyGoods();
        }

        [Test]
        public void When_Clone_Called()
        {
            BuyGoods buyGoods = m_buyGoods.Clone();

            Assert.That(buyGoods, Is.Not.Null);
            Assert.That(m_buyGoods, Is.Not.EqualTo(buyGoods));
        }

        [Test]
        public void When_Apply_Called()
        {
            m_buyGoods.Apply(m_gameContext);

            Player player = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnBuildingCostCalculated>>());
        }

        [Test]
        public void When_OnBuildingCostCalculated_Called_And_Buyer_Same_As_Player()
        {
            List<BuyGoodItem> buyGoodItems = [new BuyGoodItem() { GoodType = "Clay", MoneyCost = 2 }, new BuyGoodItem() { GoodType = "Stone", MoneyCost = 1 }];
            m_buyGoods.BuyGoodItems.AddRange(buyGoodItems);
            OnBuildingCostCalculated onBuildingCostCalculated = new OnBuildingCostCalculated(m_player);
            m_eventManager.When((arg) => arg.Subscribe(Arg.Any<Action<OnBuildingCostCalculated>>())).Do((callinfo) => ((Action<OnBuildingCostCalculated>)callinfo[0])(onBuildingCostCalculated));

            m_buyGoods.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnBuildingCostCalculated>>());
            Assert.That(buyGoodItems.All(onBuildingCostCalculated.BuyGoodItems.Contains), Is.True);
        }

        [Test]
        public void When_OnBuildingCostCalculated_Called_And_Buyer_Different_Than_Player()
        {
            List<BuyGoodItem> buyGoodItems = [new BuyGoodItem() { GoodType = "Clay", MoneyCost = 2 }, new BuyGoodItem() { GoodType = "Stone", MoneyCost = 1 }];
            m_buyGoods.BuyGoodItems.AddRange(buyGoodItems);
            OnBuildingCostCalculated onBuildingCostCalculated = new OnBuildingCostCalculated(new Player());
            m_eventManager.When((arg) => arg.Subscribe(Arg.Any<Action<OnBuildingCostCalculated>>())).Do((callinfo) => ((Action<OnBuildingCostCalculated>)callinfo[0])(onBuildingCostCalculated));

            m_buyGoods.Apply(m_gameContext);

            _ = m_turnHandler.Received(1).CurrentPlayer;
            m_eventManager.Received(1).Subscribe(Arg.Any<Action<OnBuildingCostCalculated>>());
            Assert.That(buyGoodItems.All((item) => !onBuildingCostCalculated.BuyGoodItems.Contains(item)), Is.True);
        }

        private IEventManager m_eventManager;
        private IGameContext m_gameContext;
        private ITurnHandler m_turnHandler;
        private Player m_player;
        private BuyGoods m_buyGoods;
    }
}
