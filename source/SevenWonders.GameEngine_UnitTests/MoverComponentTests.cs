using NUnit.Framework;
using System.Numerics;
using SevenWonders.GameEngine;
using System.Reflection;
using SevenWonders.GameEngine.Animations;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class MoverComponentTests
    {
        private Movement _mover;
        private GameObject _obj;
        private GameObject _target;

        [SetUp]
        public void SetUp()
        {
            _obj = new GameObject { Position = new Vector2(0, 0), Rotation = 0 };
            _target = new GameObject { Position = new Vector2(100, 0), Rotation = 90 };
        }

        [Test]
        public void MoveTo_ShouldAddMovementToList()
        {
            // Act
            _mover = new Movement(_obj, _target, 1.0f);
            _mover.Start();

            // Assert - Reflection-nel ellenőrizzük a privát listát
            var movementsField = typeof(Movement).GetField("m_movements", BindingFlags.NonPublic | BindingFlags.Instance);
            var list = (System.Collections.IList)movementsField.GetValue(_mover);

            Assert.That(list.Count, Is.EqualTo(1));
        }

        [Test]
        public void Update_ShouldMoveGameObjectTowardsTarget()
        {
            // Arrange
            float deltaTime = 1f; // 1 másodperc alatt 50 egységet kell mozognia
            _mover = new Movement(_obj, _target, 1.0f);
            _mover.Start();

            // Act
            _mover.OnUpdate(deltaTime);

            // Assert
            Assert.That(_obj.Position.X, Is.EqualTo(50f));
            Assert.That(_obj.Position.Y, Is.EqualTo(0f));
        }

        [Test]
        public void Update_ShouldRemoveMovement_WhenTargetReached()
        {
            // Arrange
            _mover = new Movement(_obj, _target, 1.0f);
            _mover.Start(); // Nagyon gyors mozgás

            // Act
            _mover.OnUpdate(0.1f); // Eléri a célt

            // Assert
            var movementsField = typeof(Movement).GetField("m_movements", BindingFlags.NonPublic | BindingFlags.Instance);
            var list = (System.Collections.IList)movementsField.GetValue(_mover);

            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(0));
            Assert.That(_obj.Position, Is.EqualTo(_target.Position));
        }
    }
}