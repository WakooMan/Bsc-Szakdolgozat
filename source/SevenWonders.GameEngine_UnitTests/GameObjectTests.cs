
using SevenWonders.GameEngine;
using System.Numerics;

namespace SevenWonders.GameEngine_UnitTests
{
    public class GameObjectTests
    {
        [Test]
        public void Constructor_ShouldInitializeDefaults()
        {
            // Act
            var obj = new GameObject();

            // Assert
            Assert.That(string.Empty, Is.EqualTo(obj.Name));
            Assert.That(obj.Animations, Is.Empty);
            Assert.That(0, Is.EqualTo(obj.CurrentAnim));
        }

        [Test]
        public void CopyConstructor_ShouldCreateDeepCopy()
        {
            // Arrange
            var original = new GameObject
            {
                Name = "TestObj",
                Id = 1,
                Position = new Vector2(10, 20),
                Scale = new Vector2(1, 1),
                Width = 100,
                Height = 100
            };

            // Act
            var copy = new GameObject(original);

            // Assert
            Assert.That(original.Name, Is.EqualTo(copy.Name));
            Assert.That(original.Id, Is.EqualTo(copy.Id));
            Assert.That(original.Position, Is.EqualTo(copy.Position));
            Assert.That(ReferenceEquals(original, copy), Is.False);
        }

        [Test]
        public void Equals_SameValues_ShouldReturnTrue()
        {
            // Arrange
            var obj1 = new GameObject { Id = 1, Name = "Player" };
            var obj2 = new GameObject { Id = 1, Name = "Player" };

            // Assert
            Assert.That(obj1, Is.EqualTo(obj2));
            Assert.That(obj1.GetHashCode(), Is.EqualTo(obj2.GetHashCode()));
        }

        [TestCase(10, 10, true)]
        [TestCase(5, 5, false)]
        [TestCase(25, 25, true)]
        [TestCase(30, 30, true)]
        [TestCase(31, 31, false)]
        public void IsTouchInGameObject_ShouldDetectBoundsCorrectly(float touchX, float touchY, bool expectedResult)
        {
            // Arrange
            var obj = new GameObject
            {
                Position = new Vector2(10, 10),
                Width = 20,
                Height = 20,
                Scale = new Vector2(1, 1)
            };

            // Act
            var method = typeof(GameObject).GetMethod("IsTouchInGameObject",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var result = (bool)method.Invoke(obj, new object[] { touchX, touchY });

            // Assert
            Assert.That(expectedResult, Is.EqualTo(result));
        }

        [Test]
        public void Resize_ShouldScalePositionAndScaleCorrectly()
        {
            // Arrange
            var obj = new GameObject
            {
                Position = new Vector2(100, 100),
                Scale = new Vector2(1, 1)
            };
            var oldRes = new Vector2(800, 600);
            var newRes = new Vector2(1600, 1200);

            // Act
            obj.Resize(oldRes, newRes);

            // Assert
            Assert.That(new Vector2(200, 200), Is.EqualTo(obj.Position));
            Assert.That(new Vector2(2, 2), Is.EqualTo(obj.Scale));
        }

        [Test]
        public void Draw_ShouldReturnImmediatelyIfNoAnimations()
        {
            // Arrange
            var obj = new GameObject { Visible = true };
            obj.Draw(null);
        }
    }
}