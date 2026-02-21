using NUnit.Framework;
using System.Numerics;
using System.Collections.Generic;
using SevenWonders.GameEngine;
using System.Reflection;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class CardFlipComponentTests
    {
        private CardFlipComponent _cardFlipComponent;
        private GameObject _card;

        [SetUp]
        public void SetUp()
        {
            _cardFlipComponent = new CardFlipComponent();
            _card = new GameObject
            {
                Scale = new Vector2(1f, 1f),
                CurrentAnim = 0 // Tegyük fel, hogy ez a hátlap
            };
        }

        [Test]
        public void Update_ShouldReduceScaleX()
        {
            // Arrange
            _cardFlipComponent.Flip(_card, spriteNumber: 1, flipSpeed: 2.0f);

            // Act
            _cardFlipComponent.Update(0.1f); // 2.0 * 0.1 = 0.2 egységnyi csökkenés

            // Assert
            Assert.That(_card.Scale.X, Is.EqualTo(0.8f));
        }

        [Test]
        public void Update_ShouldSwitchAnimation_WhenScaleXCrossesZero()
        {
            // Arrange
            // Kezdő skála 0.1, a lépésköz pedig 0.2 lesz, így átugorja a nullát
            _card.Scale = new Vector2(0.1f, 1f);
            _cardFlipComponent.Flip(_card, spriteNumber: 5, flipSpeed: 2.0f);

            // Act
            _cardFlipComponent.Update(0.1f);

            // Assert
            Assert.That(_card.CurrentAnim, Is.EqualTo(5));
            Assert.That(_card.Scale.X, Is.EqualTo(-0.1f));
        }

        [Test]
        public void Update_ShouldStopAndClamp_WhenFlipFinished()
        {
            // Arrange
            // Már majdnem kész a fordítás (-0.9)
            _card.Scale = new Vector2(-0.9f, 1f);
            _cardFlipComponent.Flip(_card, spriteNumber: 1, flipSpeed: 5.0f);

            // Act
            _cardFlipComponent.Update(0.1f); // 5.0 * 0.1 = 0.5 csökkenés -> -1.4 lenne

            // Assert
            Assert.That(_card.Scale.X, Is.EqualTo(-1f));

            // Ellenőrizzük, hogy kikerült-e a listából (Reflection)
            var field = typeof(CardFlipComponent).GetField("m_flips", BindingFlags.NonPublic | BindingFlags.Instance);
            var list = (System.Collections.IList)field.GetValue(_cardFlipComponent);

            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(0));
        }
    }
}