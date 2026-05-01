using SevenWonders.Game.Engine;
using SevenWonders.Game.Engine.Animations;
using System.Numerics;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class MoverComponentTests
    {

        [SetUp]
        public void SetUp()
        {
            m_obj = new GameObject { Position = new Vector2(0, 0), Rotation = 0 };
            m_target = new GameObject { Position = new Vector2(100, 0), Rotation = 90 };
        }

        [Test]
        public void Update_ShouldMoveGameObjectTowardsTarget()
        {
            // Arrange
            float deltaTime = 0.5f;
            m_mover = new Movement(m_obj, m_target, 1.0f);
            m_mover.Start();

            // Act
            m_mover.OnUpdate(deltaTime);

            // Assert
            Assert.That(m_obj.Position.X, Is.EqualTo(50f));
            Assert.That(m_obj.Position.Y, Is.EqualTo(0f));
            Assert.That(m_mover.IsPlaying, Is.True);
        }

        [Test]
        public void Update_IsPlaying_False_WhenTargetReached()
        {
            // Arrange
            float deltaTime = 1f;
            m_mover = new Movement(m_obj, m_target, 1.0f);
            m_mover.Start();

            // Act
            m_mover.OnUpdate(deltaTime);

            // Assert
            Assert.That(m_obj.Position.X, Is.EqualTo(100f));
            Assert.That(m_obj.Position.Y, Is.EqualTo(0f));
            Assert.That(m_mover.IsPlaying, Is.False);
        }

        private Movement m_mover;
        private GameObject m_obj;
        private GameObject m_target;
    }
}