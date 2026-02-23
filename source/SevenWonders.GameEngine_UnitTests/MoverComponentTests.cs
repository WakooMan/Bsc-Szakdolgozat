using NUnit.Framework;
using System.Numerics;
using SevenWonders.GameEngine;
using System.Reflection;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class MoverComponentTests
    {
        private MoverComponent _mover;
        private GameObject _obj;
        private GameObject _target;

        [SetUp]
        public void SetUp()
        {
            _mover = new MoverComponent();
            _obj = new GameObject { Position = new Vector2(0, 0), Rotation = 0 };
            _target = new GameObject { Position = new Vector2(100, 0), Rotation = 90 };
        }

        [Test]
        public void MoveTo_ShouldAddMovementToList()
        {
            // Act
            _mover.MoveTo(_obj, _target, 10f, 10f);

            // Assert - Reflection-nel ellenőrizzük a privát listát
            var movementsField = typeof(MoverComponent).GetField("m_movements", BindingFlags.NonPublic | BindingFlags.Instance);
            var list = (System.Collections.IList)movementsField.GetValue(_mover);

            Assert.That(list.Count, Is.EqualTo(1));
        }

        [Test]
        public void Update_ShouldMoveGameObjectTowardsTarget()
        {
            // Arrange
            float speed = 50f;
            float deltaTime = 1f; // 1 másodperc alatt 50 egységet kell mozognia
            _mover.MoveTo(_obj, _target, speed, 0f);

            // Act
            _mover.Update(deltaTime);

            // Assert
            Assert.That(_obj.Position.X, Is.EqualTo(50f));
            Assert.That(_obj.Position.Y, Is.EqualTo(0f));
        }

        [Test]
        public void Update_ShouldRemoveMovement_WhenTargetReached()
        {
            // Arrange
            _mover.MoveTo(_obj, _target, 1000f, 1000f); // Nagyon gyors mozgás

            // Act
            _mover.Update(0.1f); // Eléri a célt

            // Assert
            var movementsField = typeof(MoverComponent).GetField("m_movements", BindingFlags.NonPublic | BindingFlags.Instance);
            var list = (System.Collections.IList)movementsField.GetValue(_mover);

            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(0));
            Assert.That(_obj.Position, Is.EqualTo(_target.Position));
        }

        //[Test]
        //[TestCase(0, 90, 10, 1, 10)]    // Sima forgás 10 fokot
        //[TestCase(350, 10, 30, 1, 20)]  // Átfordulás 360-on keresztül (350 -> 360/0 -> 10)
        //[TestCase(10, 350, 30, 1, 340)] // Átfordulás visszafelé
        //public void GetNewRotation_LogicTest(float current, float target, float speed, float dt, float expected)
        //{
        //    // Privát metódus tesztelése Reflection-nel
        //    var method = typeof(MoverComponent).GetMethod("GetNewRotation", BindingFlags.NonPublic | BindingFlags.Instance);

        //    var result = (float)method.Invoke(_mover, new object[] { current, target, speed, dt, 0.01f });

        //    Assert.That(expected, Is.EqualTo(result));
        //}

        [Test]
        public void Shutdown_ShouldClearMovements()
        {
            // Arrange
            _mover.MoveTo(_obj, _target, 10, 10);

            // Act
            _mover.Shutdown();

            // Assert
            var movementsField = typeof(MoverComponent).GetField("m_movements", BindingFlags.NonPublic | BindingFlags.Instance);
            var list = (System.Collections.IList)movementsField.GetValue(_mover);

            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(0));
        }
    }
}