using NUnit.Framework;
using NSubstitute;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SevenWonders.GameEngine;
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

            // Egy kamu event args létrehozása (SkiaSharp-ban ez trükkös, 
            // de ha az interfész engedi, mockoljuk az EventArgs-ot)
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

            // 1. Lépés: Pressed beküldése (időmérés indul)
            var pressedArgs = Substitute.For<SKTouchEventArgs>(1, SKTouchAction.Pressed, new SKPoint(0, 0), true);
            _inputManager.OnTouchEvent(pressedArgs);

            // 2. Lépés: Released beküldése gyorsan (küszöbön belül)
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

            // Szimuláljunk egy hosszú várakozást (mivel a DateTime.Now-t használja a kód, 
            // a tesztben egy Thread.Sleep-re lehet szükség, vagy a DateTime absztrakciójára)
            System.Threading.Thread.Sleep(600);

            var releasedArgs = Substitute.For<SKTouchEventArgs>(1, SKTouchAction.Released, SKPoint.Empty, true);

            // Act
            _inputManager.OnTouchEvent(releasedArgs);

            // Assert
            Assert.That(clickedFired, Is.False);
        }
    }
}