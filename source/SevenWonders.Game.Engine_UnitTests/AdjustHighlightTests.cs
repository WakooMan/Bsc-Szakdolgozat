using SevenWonders.Game.Engine;
using SevenWonders.Game.Engine.Animations;
using System.Numerics;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class AdjustHighlightTests
    {
        private GameObject m_gameObject;

        [SetUp]
        public void Setup()
        {
            m_gameObject = new GameObject
            {
                VisualSize = new Vector2(1.0f, 1.0f),
                Highlighted = false
            };
        }

        [Test]
        public void When_Constructor_Called()
        {
            var anim = new AdjustHighlight(m_gameObject, new Vector2(2.0f, 2.0f), true, 1.0f);

            Assert.That(anim.IsPlaying, Is.False);
        }

        [Test]
        public void When_Start_Called()
        {
            var anim = new AdjustHighlight(m_gameObject, new Vector2(2.0f, 2.0f), true, 1.0f);

            anim.Start();

            Assert.That(anim.IsPlaying, Is.True);
        }

        [Test]
        public void When_OnUpdate_NotPlaying_ShouldDoNothing()
        {
            var anim = new AdjustHighlight(m_gameObject, new Vector2(2.0f, 2.0f), true, 1.0f);

            anim.OnUpdate(0.5f);

            Assert.Multiple(() =>
            {
                Assert.That(m_gameObject.VisualSize, Is.EqualTo(new Vector2(1.0f, 1.0f)));
                Assert.That(m_gameObject.Highlighted, Is.False);
            });
        }

        [Test]
        public void When_OnUpdate_ShouldInterpolateVisualSize()
        {
            var anim = new AdjustHighlight(m_gameObject, new Vector2(3.0f, 3.0f), true, 1.0f);
            anim.Start();

            anim.OnUpdate(0.5f);

            Assert.That(m_gameObject.VisualSize.X, Is.EqualTo(2.0f).Within(0.001f));
            Assert.That(m_gameObject.VisualSize.Y, Is.EqualTo(2.0f).Within(0.001f));
        }

        [Test]
        public void When_OnUpdate_Complete_ShouldSetHighlightAndStopPlaying()
        {
            var anim = new AdjustHighlight(m_gameObject, new Vector2(2.0f, 2.0f), true, 1.0f);
            anim.Start();

            anim.OnUpdate(1.0f);

            Assert.Multiple(() =>
            {
                Assert.That(anim.IsPlaying, Is.False);
                Assert.That(m_gameObject.Highlighted, Is.True);
                Assert.That(m_gameObject.VisualSize, Is.EqualTo(new Vector2(2.0f, 2.0f)));
            });
        }

        [Test]
        public void When_OnUpdate_Complete_ShouldSetHighlightToFalse()
        {
            m_gameObject.Highlighted = true;
            m_gameObject.VisualSize = new Vector2(2.0f, 2.0f);
            var anim = new AdjustHighlight(m_gameObject, new Vector2(1.0f, 1.0f), false, 0.5f);
            anim.Start();

            anim.OnUpdate(0.5f);

            Assert.Multiple(() =>
            {
                Assert.That(anim.IsPlaying, Is.False);
                Assert.That(m_gameObject.Highlighted, Is.False);
                Assert.That(m_gameObject.VisualSize, Is.EqualTo(new Vector2(1.0f, 1.0f)));
            });
        }

        [Test]
        public void When_OnUpdate_OvershootTime_ShouldClampToTarget()
        {
            var anim = new AdjustHighlight(m_gameObject, new Vector2(5.0f, 5.0f), true, 1.0f);
            anim.Start();

            anim.OnUpdate(2.0f);

            Assert.Multiple(() =>
            {
                Assert.That(m_gameObject.VisualSize, Is.EqualTo(new Vector2(5.0f, 5.0f)));
                Assert.That(anim.IsPlaying, Is.False);
            });
        }

        [Test]
        public void When_OnUpdate_PartialSteps_ShouldAccumulateTime()
        {
            var anim = new AdjustHighlight(m_gameObject, new Vector2(3.0f, 3.0f), true, 1.0f);
            anim.Start();

            anim.OnUpdate(0.25f);
            Assert.That(m_gameObject.VisualSize.X, Is.EqualTo(1.5f).Within(0.001f));
            Assert.That(anim.IsPlaying, Is.True);

            anim.OnUpdate(0.25f);
            Assert.That(m_gameObject.VisualSize.X, Is.EqualTo(2.0f).Within(0.001f));
            Assert.That(anim.IsPlaying, Is.True);

            anim.OnUpdate(0.5f);
            Assert.That(m_gameObject.VisualSize.X, Is.EqualTo(3.0f).Within(0.001f));
            Assert.That(anim.IsPlaying, Is.False);
        }
    }
}
