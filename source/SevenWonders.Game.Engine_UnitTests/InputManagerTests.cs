using NUnit.Framework;
using NSubstitute;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SevenWonders.Game.Engine;
using System;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class InputManagerTests
    {
        private InputManager _inputManager;
        private bool _eventFired;

        [SetUp]
        public void SetUp()
        {
            _inputManager = new InputManager();
            _eventFired = false;
        }

        [Test]
        public void Subscribe_And_Invoke_ShouldCallAction()
        {
            // Arrange
            Action<SKTouchEventArgs> action = (args) => _eventFired = true;
            _inputManager.SubscribeTouchEvent(TouchEvent.Moved, SKMouseButton.Left, action);

            var args = Substitute.For<SKTouchEventArgs>(
                1, SKTouchAction.Moved, SKPoint.Empty, true);

            // Act
            _inputManager.OnTouchEvent(args);

            // Assert
            Assert.That(_eventFired, Is.True);
        }

        [Test]
        public void Unsubscribe_ShouldPreventActionCall()
        {
            // Arrange
            Action<SKTouchEventArgs> action = (args) => _eventFired = true;
            _inputManager.SubscribeTouchEvent(TouchEvent.Pressed, SKMouseButton.Left, action);
            _inputManager.UnsubscribeTouchEvent(TouchEvent.Pressed, SKMouseButton.Left, action);

            var args = Substitute.For<SKTouchEventArgs>(
                1, SKTouchAction.Pressed, SKPoint.Empty, true);

            // Act
            _inputManager.OnTouchEvent(args);

            // Assert
            Assert.That(_eventFired, Is.False);
        }

        [Test]
        public void Released_WithinThreshold_ShouldFireClickedEvent()
        {
            // Arrange
            bool clickedFired = false;
            _inputManager.SubscribeTouchEvent(TouchEvent.Clicked, SKMouseButton.Left, (args) => clickedFired = true);

            var pressedArgs = Substitute.For<SKTouchEventArgs>(1, SKTouchAction.Pressed, new SKPoint(0, 0), true);
            _inputManager.OnTouchEvent(pressedArgs);

            var releasedArgs = Substitute.For<SKTouchEventArgs>(1, SKTouchAction.Released, new SKPoint(2, 2), true);

            // Act
            _inputManager.OnTouchEvent(releasedArgs);

            // Assert
            Assert.That(clickedFired, Is.True);
        }

        [Test]
        public void Released_AfterLongTime_ShouldNotFireClickedEvent()
        {
            // Arrange
            bool clickedFired = false;
            _inputManager.SubscribeTouchEvent(TouchEvent.Clicked, SKMouseButton.Left, (args) => clickedFired = true);

            _inputManager.OnTouchEvent(Substitute.For<SKTouchEventArgs>(1, SKTouchAction.Pressed, SKPoint.Empty, true));

            Thread.Sleep(600);

            var releasedArgs = Substitute.For<SKTouchEventArgs>(1, SKTouchAction.Released, SKPoint.Empty, true);

            // Act
            _inputManager.OnTouchEvent(releasedArgs);

            // Assert
            Assert.That(clickedFired, Is.False);
        }
    }
}