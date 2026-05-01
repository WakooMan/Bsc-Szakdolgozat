using SevenWonders.Game.Engine;
using System.Numerics;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class ChildTextLabelTests
    {
        [Test]
        public void When_DefaultConstructor_Called()
        {
            var ctl = new ChildTextLabel();

            Assert.Multiple(() =>
            {
                Assert.That(ctl.TextLabel, Is.Not.Null);
                Assert.That(ctl.WidthPercent, Is.EqualTo(0f));
                Assert.That(ctl.HeightPercent, Is.EqualTo(0f));
                Assert.That(ctl.PositionPercent, Is.EqualTo(Vector2.Zero));
            });
        }

        [Test]
        public void When_CopyConstructor_Called()
        {
            var original = new ChildTextLabel
            {
                WidthPercent = 0.5f,
                HeightPercent = 0.3f,
                PositionPercent = new Vector2(0.1f, 0.2f),
                TextLabel = new TextLabel { Text = "Hello", FontSize = 16f }
            };

            var copy = new ChildTextLabel(original);

            Assert.Multiple(() =>
            {
                Assert.That(copy.WidthPercent, Is.EqualTo(original.WidthPercent));
                Assert.That(copy.HeightPercent, Is.EqualTo(original.HeightPercent));
                Assert.That(copy.PositionPercent, Is.EqualTo(original.PositionPercent));
                Assert.That(copy.TextLabel.Text, Is.EqualTo("Hello"));
                Assert.That(ReferenceEquals(copy.TextLabel, original.TextLabel), Is.False);
            });
        }

        [Test]
        public void When_Equals_Called_SameValues_ShouldReturnTrue()
        {
            var a = new ChildTextLabel
            {
                WidthPercent = 0.5f,
                HeightPercent = 0.3f,
                PositionPercent = new Vector2(0.1f, 0.2f),
                TextLabel = new TextLabel { Id = 1, Name = "Lbl" }
            };
            var b = new ChildTextLabel
            {
                WidthPercent = 0.5f,
                HeightPercent = 0.3f,
                PositionPercent = new Vector2(0.1f, 0.2f),
                TextLabel = new TextLabel { Id = 1, Name = "Lbl" }
            };

            Assert.That(a.Equals(b), Is.True);
            Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
        }

        [Test]
        public void When_Equals_Called_DifferentTextLabel_ShouldReturnFalse()
        {
            var a = new ChildTextLabel
            {
                WidthPercent = 0.5f,
                TextLabel = new TextLabel { Id = 1, Name = "A" }
            };
            var b = new ChildTextLabel
            {
                WidthPercent = 0.5f,
                TextLabel = new TextLabel { Id = 2, Name = "B" }
            };

            Assert.That(a.Equals(b), Is.False);
        }

        [Test]
        public void When_Equals_Called_With_Null_ShouldReturnFalse()
        {
            var ctl = new ChildTextLabel();
            Assert.That(ctl.Equals((ChildTextLabel?)null), Is.False);
        }

        [Test]
        public void When_Equals_Called_ChildObject_OfDifferentType_ShouldReturnFalse()
        {
            var ctl = new ChildTextLabel();
            var ct = new ChildTexture();

            Assert.That(ctl.Equals((ChildObject)ct), Is.False);
        }

        [Test]
        public void When_Equals_Called_ChildObject_OfSameType_ShouldDelegate()
        {
            var a = new ChildTextLabel
            {
                WidthPercent = 0.5f,
                TextLabel = new TextLabel { Id = 1 }
            };
            var b = new ChildTextLabel
            {
                WidthPercent = 0.5f,
                TextLabel = new TextLabel { Id = 1 }
            };

            Assert.That(a.Equals((ChildObject)b), Is.True);
        }

        [Test]
        public void When_Equals_Called_Object_OfSameType_ShouldReturnTrue()
        {
            var a = new ChildTextLabel
            {
                WidthPercent = 0.5f,
                TextLabel = new TextLabel { Id = 1 }
            };
            var b = new ChildTextLabel
            {
                WidthPercent = 0.5f,
                TextLabel = new TextLabel { Id = 1 }
            };

            Assert.That(a.Equals((object)b), Is.True);
        }

        [Test]
        public void When_Equals_Called_Object_OfDifferentType_ShouldReturnFalse()
        {
            var ctl = new ChildTextLabel();
            Assert.That(ctl.Equals(new object()), Is.False);
        }
    }
}
