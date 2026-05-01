using NSubstitute;
using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Guilds;
using SevenWonders.Game.Logic.Elements.Wonders;
using SevenWonders.Game.Logic.Interfaces;

namespace GameLogic_UnitTests.Elements.Guilds
{
    [TestFixture]
    public class GuildTests
    {
        private IGameContext m_gameContext;
        private IPlayerActionReceiver m_receiver;
        private Player m_owner;
        private Player m_opponent;

        [SetUp]
        public void Setup()
        {
            m_gameContext = Substitute.For<IGameContext>();
            m_receiver = Substitute.For<IPlayerActionReceiver>();
            m_owner = new Player(m_receiver, "Owner", 1, 10);
            m_opponent = new Player(m_receiver, "Opponent", 2, 10);
        }

        [Test]
        public void When_BuilderGuild_Clone_ShouldReturnNewInstance()
        {
            var guild = new BuilderGuild();
            var clone = guild.Clone();

            Assert.That(clone, Is.TypeOf<BuilderGuild>());
            Assert.That(ReferenceEquals(clone, guild), Is.False);
        }

        [Test]
        public void When_BuilderGuild_CalculateGuildVP_OwnerHasMoreBuiltWonders()
        {
            m_owner.Wonders.Add(new Wonder { HasBeenBuilt = true });
            m_owner.Wonders.Add(new Wonder { HasBeenBuilt = true });
            m_owner.Wonders.Add(new Wonder { HasBeenBuilt = false });
            m_opponent.Wonders.Add(new Wonder { HasBeenBuilt = true });

            var guild = new BuilderGuild();
            var props = new PlayerProperties(m_owner, m_opponent);

            Assert.That(guild.CalculateGuildVP(props), Is.EqualTo(4));
        }

        [Test]
        public void When_BuilderGuild_CalculateGuildVP_OpponentHasMoreBuiltWonders()
        {
            m_owner.Wonders.Add(new Wonder { HasBeenBuilt = true });
            m_opponent.Wonders.Add(new Wonder { HasBeenBuilt = true });
            m_opponent.Wonders.Add(new Wonder { HasBeenBuilt = true });
            m_opponent.Wonders.Add(new Wonder { HasBeenBuilt = true });

            var guild = new BuilderGuild();
            var props = new PlayerProperties(m_owner, m_opponent);

            Assert.That(guild.CalculateGuildVP(props), Is.EqualTo(6));
        }

        [Test]
        public void When_BuilderGuild_CalculateGuildVP_NoBuiltWonders_ShouldReturnZero()
        {
            var guild = new BuilderGuild();
            var props = new PlayerProperties(m_owner, m_opponent);

            Assert.That(guild.CalculateGuildVP(props), Is.EqualTo(0));
        }

        [Test]
        public void When_BuilderGuild_OnCalculatePlayerProperties_ShouldAddVP()
        {
            m_owner.Wonders.Add(new Wonder { HasBeenBuilt = true });
            m_opponent.Wonders.Add(new Wonder { HasBeenBuilt = true });

            var guild = new BuilderGuild();
            var props = new PlayerProperties(m_owner, m_opponent);

            guild.OnCalculatePlayerProperties(props);

            Assert.That(props.VictoryPoints, Is.EqualTo(2));
        }

        [Test]
        public void When_MagistrateGuild_Clone_ShouldReturnNewInstance()
        {
            var guild = new MagistrateGuild();
            Assert.That(guild.Clone(), Is.TypeOf<MagistrateGuild>());
        }

        [Test]
        public void When_MagistrateGuild_CalculateGuildVP_ShouldReturnMaxBlueCards()
        {
            m_owner.Cards.Add(new BlueCard());
            m_opponent.Cards.Add(new BlueCard());
            m_opponent.Cards.Add(new BlueCard());

            var guild = new MagistrateGuild();
            var props = new PlayerProperties(m_owner, m_opponent);

            Assert.That(guild.CalculateGuildVP(props), Is.EqualTo(2));
        }

        [Test]
        public void When_MagistrateGuild_CalculateMoney_ShouldReturnMaxBlueCards()
        {
            m_owner.Cards.Add(new BlueCard());
            m_owner.Cards.Add(new BlueCard());
            m_owner.Cards.Add(new BlueCard());

            var guild = new MagistrateGuild();
            var props = new PlayerProperties(m_owner, m_opponent);

            Assert.That(guild.CalculateMoney(props), Is.EqualTo(3));
        }

        [Test]
        public void When_MagistrateGuild_Apply_ShouldAddMoneyToOwner()
        {
            m_owner.Cards.Add(new BlueCard());
            m_owner.Cards.Add(new BlueCard());

            var guild = new MagistrateGuild();
            guild.Apply(m_gameContext, m_owner, m_opponent);

            Assert.That(m_owner.Money, Is.EqualTo(12));
        }

        [Test]
        public void When_MagistrateGuild_OnCalculatePlayerProperties_ShouldAddVP()
        {
            m_owner.Cards.Add(new BlueCard());
            m_owner.Cards.Add(new BlueCard());

            var guild = new MagistrateGuild();
            var props = new PlayerProperties(m_owner, m_opponent);
            guild.OnCalculatePlayerProperties(props);

            Assert.That(props.VictoryPoints, Is.EqualTo(2));
        }

        [Test]
        public void When_SailorGuild_Clone_ShouldReturnNewInstance()
        {
            Assert.That(new SailorGuild().Clone(), Is.TypeOf<SailorGuild>());
        }

        [Test]
        public void When_SailorGuild_CalculateGuildVP_ShouldReturnMaxGrayAndBrownCards()
        {
            m_owner.Cards.Add(new GrayCard());
            m_owner.Cards.Add(new BrownCard());
            m_opponent.Cards.Add(new GrayCard());

            var guild = new SailorGuild();
            var props = new PlayerProperties(m_owner, m_opponent);

            Assert.That(guild.CalculateGuildVP(props), Is.EqualTo(2));
        }

        [Test]
        public void When_SailorGuild_CalculateGuildVP_ShouldIgnoreOtherCardTypes()
        {
            m_owner.Cards.Add(new GrayCard());
            m_owner.Cards.Add(new RedCard());
            m_opponent.Cards.Add(new BrownCard());
            m_opponent.Cards.Add(new BrownCard());
            m_opponent.Cards.Add(new BrownCard());

            var guild = new SailorGuild();
            var props = new PlayerProperties(m_owner, m_opponent);

            Assert.That(guild.CalculateGuildVP(props), Is.EqualTo(3));
        }

        [Test]
        public void When_SailorGuild_CalculateMoney_ShouldReturnMaxGrayBrownCards()
        {
            m_opponent.Cards.Add(new GrayCard());
            m_opponent.Cards.Add(new GrayCard());

            var guild = new SailorGuild();
            var props = new PlayerProperties(m_owner, m_opponent);

            Assert.That(guild.CalculateMoney(props), Is.EqualTo(2));
        }

        [Test]
        public void When_SailorGuild_Apply_ShouldAddMoneyToOwner()
        {
            m_owner.Cards.Add(new BrownCard());
            m_owner.Cards.Add(new BrownCard());
            m_owner.Cards.Add(new GrayCard());

            var guild = new SailorGuild();
            guild.Apply(m_gameContext, m_owner, m_opponent);

            Assert.That(m_owner.Money, Is.EqualTo(13));
        }

        [Test]
        public void When_SailorGuild_OnCalculatePlayerProperties_ShouldAddVP()
        {
            m_owner.Cards.Add(new BrownCard());

            var guild = new SailorGuild();
            var props = new PlayerProperties(m_owner, m_opponent);
            guild.OnCalculatePlayerProperties(props);

            Assert.That(props.VictoryPoints, Is.EqualTo(1));
        }

        [Test]
        public void When_ScienceGuild_Clone_ShouldReturnNewInstance()
        {
            Assert.That(new ScienceGuild().Clone(), Is.TypeOf<ScienceGuild>());
        }

        [Test]
        public void When_ScienceGuild_CalculateGuildVP_ShouldReturnMaxGreenCards()
        {
            m_opponent.Cards.Add(new GreenCard());
            m_opponent.Cards.Add(new GreenCard());

            var guild = new ScienceGuild();
            var props = new PlayerProperties(m_owner, m_opponent);

            Assert.That(guild.CalculateGuildVP(props), Is.EqualTo(2));
        }

        [Test]
        public void When_ScienceGuild_CalculateMoney_ShouldReturnMaxGreenCards()
        {
            m_owner.Cards.Add(new GreenCard());
            m_owner.Cards.Add(new GreenCard());
            m_owner.Cards.Add(new GreenCard());

            var guild = new ScienceGuild();
            var props = new PlayerProperties(m_owner, m_opponent);

            Assert.That(guild.CalculateMoney(props), Is.EqualTo(3));
        }

        [Test]
        public void When_ScienceGuild_Apply_ShouldAddMoneyToOwner()
        {
            m_owner.Cards.Add(new GreenCard());

            var guild = new ScienceGuild();
            guild.Apply(m_gameContext, m_owner, m_opponent);

            Assert.That(m_owner.Money, Is.EqualTo(11));
        }

        [Test]
        public void When_ScienceGuild_OnCalculatePlayerProperties_ShouldAddVP()
        {
            m_opponent.Cards.Add(new GreenCard());
            m_opponent.Cards.Add(new GreenCard());

            var guild = new ScienceGuild();
            var props = new PlayerProperties(m_owner, m_opponent);
            guild.OnCalculatePlayerProperties(props);

            Assert.That(props.VictoryPoints, Is.EqualTo(2));
        }

        [Test]
        public void When_StrategistGuild_Clone_ShouldReturnNewInstance()
        {
            Assert.That(new StrategistGuild().Clone(), Is.TypeOf<StrategistGuild>());
        }

        [Test]
        public void When_StrategistGuild_CalculateGuildVP_ShouldReturnMaxRedCards()
        {
            m_owner.Cards.Add(new RedCard());
            m_opponent.Cards.Add(new RedCard());
            m_opponent.Cards.Add(new RedCard());

            var guild = new StrategistGuild();
            var props = new PlayerProperties(m_owner, m_opponent);

            Assert.That(guild.CalculateGuildVP(props), Is.EqualTo(2));
        }

        [Test]
        public void When_StrategistGuild_CalculateMoney_ShouldReturnMaxRedCards()
        {
            m_owner.Cards.Add(new RedCard());

            var guild = new StrategistGuild();
            var props = new PlayerProperties(m_owner, m_opponent);

            Assert.That(guild.CalculateMoney(props), Is.EqualTo(1));
        }

        [Test]
        public void When_StrategistGuild_Apply_ShouldAddMoneyToOwner()
        {
            m_opponent.Cards.Add(new RedCard());
            m_opponent.Cards.Add(new RedCard());

            var guild = new StrategistGuild();
            guild.Apply(m_gameContext, m_owner, m_opponent);

            Assert.That(m_owner.Money, Is.EqualTo(12));
        }

        [Test]
        public void When_StrategistGuild_OnCalculatePlayerProperties_ShouldAddVP()
        {
            m_owner.Cards.Add(new RedCard());
            m_owner.Cards.Add(new RedCard());
            m_owner.Cards.Add(new RedCard());

            var guild = new StrategistGuild();
            var props = new PlayerProperties(m_owner, m_opponent);
            guild.OnCalculatePlayerProperties(props);

            Assert.That(props.VictoryPoints, Is.EqualTo(3));
        }

        [Test]
        public void When_TraderGuild_Clone_ShouldReturnNewInstance()
        {
            Assert.That(new TraderGuild().Clone(), Is.TypeOf<TraderGuild>());
        }

        [Test]
        public void When_TraderGuild_CalculateGuildVP_ShouldReturnMaxYellowCards()
        {
            m_owner.Cards.Add(new YellowCard());
            m_owner.Cards.Add(new YellowCard());
            m_opponent.Cards.Add(new YellowCard());

            var guild = new TraderGuild();
            var props = new PlayerProperties(m_owner, m_opponent);

            Assert.That(guild.CalculateGuildVP(props), Is.EqualTo(2));
        }

        [Test]
        public void When_TraderGuild_CalculateMoney_ShouldReturnMaxYellowCards()
        {
            m_opponent.Cards.Add(new YellowCard());
            m_opponent.Cards.Add(new YellowCard());
            m_opponent.Cards.Add(new YellowCard());

            var guild = new TraderGuild();
            var props = new PlayerProperties(m_owner, m_opponent);

            Assert.That(guild.CalculateMoney(props), Is.EqualTo(3));
        }

        [Test]
        public void When_TraderGuild_Apply_ShouldAddMoneyToOwner()
        {
            m_owner.Cards.Add(new YellowCard());
            m_owner.Cards.Add(new YellowCard());

            var guild = new TraderGuild();
            guild.Apply(m_gameContext, m_owner, m_opponent);

            Assert.That(m_owner.Money, Is.EqualTo(12));
        }

        [Test]
        public void When_TraderGuild_OnCalculatePlayerProperties_ShouldAddVP()
        {
            m_owner.Cards.Add(new YellowCard());

            var guild = new TraderGuild();
            var props = new PlayerProperties(m_owner, m_opponent);
            guild.OnCalculatePlayerProperties(props);

            Assert.That(props.VictoryPoints, Is.EqualTo(1));
        }

        [Test]
        public void When_TraderGuild_CalculateGuildVP_NoCards_ShouldReturnZero()
        {
            var guild = new TraderGuild();
            var props = new PlayerProperties(m_owner, m_opponent);

            Assert.That(guild.CalculateGuildVP(props), Is.EqualTo(0));
        }
    }
}
