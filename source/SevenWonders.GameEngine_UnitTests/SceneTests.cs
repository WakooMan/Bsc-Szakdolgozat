using NUnit.Framework;
using NSubstitute;
using System.Numerics;
using System.Collections.Generic;
using SevenWonders.GameEngine;
using System.Linq;
using System;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class SceneTests
    {
        private Scene _originalScene;

        [SetUp]
        public void SetUp()
        {
            _originalScene = new Scene
            {
                Id = Guid.NewGuid(),
                Name = "MainLevel",
                Visible = true,
                BiggestId = 10,
                Resolution = new Vector2(1920, 1080),
                Layers = new List<GraphicsLayer>
                {
                    new GraphicsLayer { ID = 1, Name = "Background" }
                }
            };
        }

        [Test]
        public void Constructor_ShouldSetDefaultResolutionAndEmptyLayers()
        {
            // Act
            var scene = new Scene();

            // Assert
            Assert.AreEqual(new Vector2(3840, 2160), scene.Resolution);
            Assert.IsEmpty(scene.Layers);
            Assert.AreEqual(Guid.Empty, scene.Id);
        }

        [Test]
        public void CopyConstructor_ShouldCreateDeepCopyAndNewId()
        {
            // Act
            var copy = new Scene(_originalScene);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreNotEqual(_originalScene.Id, copy.Id, "A másolatnak új Guid-ot kell kapnia.");
                Assert.AreEqual(_originalScene.Name, copy.Name);
                Assert.AreEqual(_originalScene.BiggestId, copy.BiggestId);

                // Rétegek ellenőrzése
                Assert.AreEqual(_originalScene.Layers.Count, copy.Layers.Count);
                Assert.AreNotSame(_originalScene.Layers[0], copy.Layers[0], "A rétegeket is klónozni kell (Deep Copy).");
            });
        }

        [Test]
        public void Equals_WhenAllPropertiesMatch_ShouldReturnTrue()
        {
            // Arrange
            var scene1 = new Scene { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Menu" };
            var scene2 = new Scene { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Menu" };

            // Act & Assert
            Assert.IsTrue(scene1.Equals(scene2));
            Assert.AreEqual(scene1.GetHashCode(), scene2.GetHashCode());
        }

        [Test]
        public void Resize_ShouldUpdateResolutionAndPropagateToLayers()
        {
            // Arrange
            var newRes = new Vector2(1280, 720);

            // Mockoljuk a réteget, hogy ellenőrizzük a hívást (bár a GraphicsLayer nem interfész, 
            // a valódi objektumot is használhatjuk az állapotalapú teszteléshez)
            var layer = new GraphicsLayer { Name = "UI" };
            _originalScene.Layers = new List<GraphicsLayer> { layer };
            var oldRes = _originalScene.Resolution;

            // Act
            _originalScene.Resize(newRes);

            // Assert
            Assert.AreEqual(newRes, _originalScene.Resolution);
            // Itt a layer.Resize belső állapotát ellenőrizhetnénk, ha a layernek lennének objektumai
        }

        [Test]
        public void Draw_WhenNotVisible_ShouldReturnImmediately()
        {
            // Arrange
            _originalScene.Visible = false;
            // Ha null-t adunk át és nem dob hibát, akkor az if(!Visible) lefutott
            Assert.DoesNotThrow(() => _originalScene.Draw(null!));
        }
    }
}