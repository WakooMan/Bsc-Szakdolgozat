using NSubstitute;
using SevenWonders.Game.Engine.Animations;
using SevenWonders.Game.Engine.Components;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class AnimationManagerTests
    {
        private AnimationManager m_manager;

        [SetUp]
        public void Setup()
        {
            m_manager = new AnimationManager();
        }

        [Test]
        public void When_Constructor_Called()
        {
            Assert.Multiple(() =>
            {
                Assert.That(m_manager.Id, Is.EqualTo(100));
                Assert.That(m_manager.Name, Is.EqualTo(nameof(AnimationManager)));
            });
        }

        [Test]
        public void When_Update_WithNoAnimations_Called_ShouldNotThrow()
        {
            Assert.DoesNotThrow(() => m_manager.Update(0.016f));
        }

        [Test]
        public void When_Enqueue_ShouldQueueAnimation_AndUpdateStartsIt()
        {
            var animation = Substitute.For<IAnimation>();
            animation.IsPlaying.Returns(true);

            m_manager.Enqueue(animation);
            m_manager.Update(0.016f);

            animation.Received(1).Start();
            animation.Received(1).OnUpdate(0.016f);
        }

        [Test]
        public void When_Update_Called_WhenAnimationStillPlaying_ShouldNotClear()
        {
            var animation = Substitute.For<IAnimation>();
            animation.IsPlaying.Returns(true);

            m_manager.Enqueue(animation);
            m_manager.Update(0.016f);
            m_manager.Update(0.016f);

            animation.Received(2).OnUpdate(0.016f);
        }

        [Test]
        public void Update_WhenAnimationFinishes_ShouldClearActive()
        {
            var animation = Substitute.For<IAnimation>();
            animation.IsPlaying.Returns(true, false);

            m_manager.Enqueue(animation);
            m_manager.Update(0.016f);
            m_manager.Update(0.016f);

            Assert.DoesNotThrow(() => m_manager.Update(0.016f));
        }

        [Test]
        public void When_EnqueueAsync_ShouldReturnTask_ThatCompletesWhenAnimationFinishes()
        {
            var animation = Substitute.For<IAnimation>();
            animation.IsPlaying.Returns(true, false);

            var task = m_manager.EnqueueAsync(animation);

            Assert.That(task.IsCompleted, Is.False);

            m_manager.Update(0.016f);
            Assert.That(task.IsCompleted, Is.False);

            m_manager.Update(0.016f);
            Assert.That(task.IsCompleted, Is.True);
        }

        [Test]
        public void When_EnqueueAsync_WithImmediateFinish_ShouldCompleteAfterOneUpdate()
        {
            var animation = Substitute.For<IAnimation>();
            animation.IsPlaying.Returns(false);

            var task = m_manager.EnqueueAsync(animation);
            m_manager.Update(0.016f);

            Assert.That(task.IsCompleted, Is.True);
        }

        [Test]
        public void When_Enqueue_MultipleGroups_ShouldPlayInSequence()
        {
            var anim1 = Substitute.For<IAnimation>();
            anim1.IsPlaying.Returns(false);

            var anim2 = Substitute.For<IAnimation>();
            anim2.IsPlaying.Returns(false);

            m_manager.Enqueue(anim1);
            m_manager.Enqueue(anim2);

            m_manager.Update(0.016f);
            anim1.Received(1).Start();

            m_manager.Update(0.016f);
            anim2.Received(1).Start();
        }

        [Test]
        public void When_Enqueue_MultipleAnimationsAtOnce_ShouldPlayAllSimultaneously()
        {
            var anim1 = Substitute.For<IAnimation>();
            var anim2 = Substitute.For<IAnimation>();
            anim1.IsPlaying.Returns(true);
            anim2.IsPlaying.Returns(true);

            m_manager.Enqueue(anim1, anim2);
            m_manager.Update(0.016f);

            anim1.Received(1).Start();
            anim2.Received(1).Start();
            anim1.Received(1).OnUpdate(0.016f);
            anim2.Received(1).OnUpdate(0.016f);
        }

        [Test]
        public void When_Enqueue_MultipleAnimationsAtOnce_ClearsOnlyWhenAllFinish()
        {
            var anim1 = Substitute.For<IAnimation>();
            var anim2 = Substitute.For<IAnimation>();
            anim1.IsPlaying.Returns(false);
            anim2.IsPlaying.Returns(true, false);

            m_manager.Enqueue(anim1, anim2);

            m_manager.Update(0.016f);
            anim1.Received(1).OnUpdate(0.016f);
            anim2.Received(1).OnUpdate(0.016f);

            m_manager.Update(0.016f);
            Assert.DoesNotThrow(() => m_manager.Update(0.016f));
        }

        [Test]
        public void When_Shutdown_ShouldClearAllAnimations()
        {
            var animation = Substitute.For<IAnimation>();
            animation.IsPlaying.Returns(true);

            m_manager.Enqueue(animation);
            m_manager.Shutdown();

            Assert.DoesNotThrow(() => m_manager.Update(0.016f));
            animation.DidNotReceive().Start();
        }

        [Test]
        public void When_Startup_ShouldClearAllAnimations()
        {
            var animation = Substitute.For<IAnimation>();
            animation.IsPlaying.Returns(true);

            m_manager.Enqueue(animation);
            m_manager.Startup();

            Assert.DoesNotThrow(() => m_manager.Update(0.016f));
            animation.DidNotReceive().Start();
        }

        [Test]
        public void When_Id_Setter_Called()
        {
            m_manager.Id = 200;
            Assert.That(m_manager.Id, Is.EqualTo(200));
        }

        [Test]
        public void When_Name_Setter_Called()
        {
            m_manager.Name = "Custom";
            Assert.That(m_manager.Name, Is.EqualTo("Custom"));
        }
    }
}
