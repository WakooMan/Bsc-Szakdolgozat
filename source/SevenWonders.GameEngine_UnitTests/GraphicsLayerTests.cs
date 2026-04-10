using NUnit.Framework;
using System.Numerics;
using System.Collections.Generic;
using SevenWonders.GameEngine;
using System.Linq;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class GraphicsLayerTests
    {
        private GraphicsLayer _originalLayer;

        [SetUp]
        public void SetUp()
        {
            _originalLayer = new GraphicsLayer
            {
                Id = 1,
                Name = "BackgroundLayer",
                Visible = true,
                ZIndex = 5,
                ObjectList = new List<GameObject>
                {
                    new GameObject { Id = 101, Name = "Player" }
                },
                TextureObjects = new List<TextureObject>
                {
                    new TextureObject { Id = 201, Name = "Grass" }
                }
            };
        }

        [Test]
        public void Constructor_ShouldInitializeEmptyLists()
        {
            // Act
            var layer = new GraphicsLayer();

            // Assert
            Assert.That(layer.ObjectList, Is.Not.Null);
            Assert.That(layer.TextureObjects, Is.Not.Null);
            Assert.That(layer.Name, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CopyConstructor_ShouldCreateDeepCopy()
        {
            // Act
            var copy = new GraphicsLayer(_originalLayer);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_originalLayer.Id, Is.EqualTo(copy.Id));
                Assert.That(_originalLayer.Name, Is.EqualTo(copy.Name));

                // Referencia ellenőrzés: nem ugyanaz az objektum
                Assert.That(ReferenceEquals(_originalLayer, copy), Is.False);

                // Lista tartalom ellenőrzés
                Assert.That(_originalLayer.ObjectList.Count, Is.EqualTo(copy.ObjectList.Count));
                Assert.That(_originalLayer.TextureObjects.Count, Is.EqualTo(copy.TextureObjects.Count));

                // Mély másolás ellenőrzése: a listában lévő objektumoknak is új példányoknak kell lenniük
                Assert.That(ReferenceEquals(_originalLayer.ObjectList[0], copy.ObjectList[0]), Is.False);
                Assert.That(ReferenceEquals(_originalLayer.TextureObjects[0], copy.TextureObjects[0]), Is.False);
            });
        }

        [Test]
        public void Equals_WhenIdenticalData_ShouldReturnTrue()
        {
            // Arrange
            var sameLayer = new GraphicsLayer(_originalLayer);

            // Act & Assert
            // A SequenceEqual miatt True-nak kell lennie, ha a belső listák elemei is egyenlőek
            Assert.That(_originalLayer, Is.EqualTo(sameLayer));
            Assert.That(_originalLayer.GetHashCode(), Is.EqualTo(sameLayer.GetHashCode()));
        }

        [Test]
        public void Equals_WhenListContentDiffers_ShouldReturnFalse()
        {
            // Arrange
            var differentLayer = new GraphicsLayer(_originalLayer);
            differentLayer.ObjectList.Add(new GameObject { Id = 999 });

            // Act & Assert
            Assert.That(_originalLayer, Is.Not.EqualTo(differentLayer));
        }

        [Test]
        public void Equals_GraphicsLayer_Null_ShouldReturnFalse()
        {
            // Arrange
            var obj1 = new GraphicsLayer { Id = 1, Name = "Layer" };
            GraphicsLayer? obj2 = null;

            // Assert
            Assert.That(obj1.Equals(obj2), Is.False);
        }

        [Test]
        public void Equals_Object_Not_GraphicsLayer_ShouldReturnFalse()
        {
            // Arrange
            var obj1 = new GraphicsLayer { Id = 1, Name = "Layer" };
            object? obj2 = new object();

            // Assert
            Assert.That(obj1.Equals(obj2), Is.False);
        }

        [Test]
        public void Resize_ShouldPropagateToAllContainedObjects()
        {
            // Arrange
            var oldRes = new Vector2(800, 600);
            var newRes = new Vector2(1600, 1200); // 2x skálázás

            var gameObject = new GameObject { Position = new Vector2(10, 10), VisualSize = new Vector2(1, 1) };
            var texture = new TextureObject { Position = new Vector2(20, 20), Width = 100 };

            _originalLayer.ObjectList = new List<GameObject> { gameObject };
            _originalLayer.TextureObjects = new List<TextureObject> { texture };

            // Act
            _originalLayer.Resize(oldRes, newRes);

            // Assert
            Assert.Multiple(() =>
            {
                // GameObject ellenőrzés
                Assert.That(gameObject.Position.X, Is.EqualTo(20f));
                // TextureObject ellenőrzés
                Assert.That(texture.Position.X, Is.EqualTo(40f));
                Assert.That(texture.Width, Is.EqualTo(200f));
            });
        }

        [Test]
        public void Draw_WhenVisibleIsFalse_ShouldNotThrowExceptionWithNullArgs()
        {
            // Arrange
            _originalLayer.Visible = false;

            // Act & Assert
            // Ha a Visible false, a kódnak azonnal vissza kell térnie, 
            // így a null eventArgs nem okozhat NullReferenceException-t.
            Assert.DoesNotThrow(() => _originalLayer.Draw(null!));
        }
    }
}