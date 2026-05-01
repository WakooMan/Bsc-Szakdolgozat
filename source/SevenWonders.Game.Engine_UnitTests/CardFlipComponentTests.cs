using SevenWonders.Game.Engine;
using SevenWonders.Game.Engine.Animations;
using System.Numerics;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class CardFlipComponentTests
    {
        [SetUp]
        public void SetUp()
        {
            m_cardFlipComponent = new CardFlip(null!, 0, 0f);
            m_card = new GameObject
            {
                VisualSize = new Vector2(1f, 1f),
                CurrentAnim = 0
            };
        }

        [Test]
        public void Update_ShouldReduceScaleX()
        {
            // Arrange
            m_cardFlipComponent = new CardFlip(m_card, spriteNumber: 1, 2.0f);
            m_cardFlipComponent.Start();

            // Act
            m_cardFlipComponent.OnUpdate(0.1f);

            // Assert
            Assert.That(m_card.FlipMultiplier.X, Is.EqualTo(0.9f));
        }

        [Test]
        public void Update_ShouldSwitchAnimation_WhenScaleXCrossesZero()
        {
            // Arrange
            m_cardFlipComponent = new CardFlip(m_card, spriteNumber: 5, 2.0f);
            m_card.FlipMultiplier = new Vector2(0.1f, 1f);
            m_cardFlipComponent.Start();

            // Act
            m_cardFlipComponent.OnUpdate(0.1f);

            // Assert
            Assert.That(m_card.CurrentAnim, Is.EqualTo(0));
            Assert.That(m_card.FlipMultiplier.X, Is.EqualTo(0.9f));
        }

        [Test]
        public void Update_ShouldStopAndClamp_WhenFlipFinished()
        {
            // Arrange
            m_cardFlipComponent = new CardFlip(m_card, spriteNumber: 1, 5.0f);
            m_card.FlipMultiplier = new Vector2(-0.9f, 1f);
            m_cardFlipComponent.Start();

            // Act
            m_cardFlipComponent.OnUpdate(0.1f);

            // Assert
            Assert.That(m_card.FlipMultiplier.X, Is.EqualTo(0.96f));
        }

        private CardFlip m_cardFlipComponent;
        private GameObject m_card;
    }
}