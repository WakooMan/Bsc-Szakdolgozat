using SevenWonders.Game.Engine;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class InputEventArgsTests
    {
        [SetUp]
        public void Setup()
        {
            m_inputEventArgs = new InputEventArgs();
        }

        [Test]
        public void When_GetArgument_Called_Returns_default_value()
        {
            string value = m_inputEventArgs.GetArgument<string>("Name");

            Assert.That(value, Is.Null);
        }

        [Test]
        public void When_AddArgument_Called_GetArgument_Returns_value()
        {
            string expectedValue = "something";

            m_inputEventArgs.AddArgument("Name", expectedValue);

            string value = m_inputEventArgs.GetArgument<string>("Name");

            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.EqualTo(expectedValue));
        }

        [Test]
        public void When_AddArgument_Called_GetArgument_Returns_default_value_Different_Type()
        {
            string addedValue = "something";
            int expectedValue = 0;

            m_inputEventArgs.AddArgument("Name", addedValue);

            int value = m_inputEventArgs.GetArgument<int>("Name");

            Assert.That(value, Is.EqualTo(expectedValue));
        }

        [Test]
        public void When_AddArgument_Called_With_Null_Arguments()
        {
            Assert.Throws<ArgumentNullException>(() => m_inputEventArgs.AddArgument(null, "something"));
            Assert.Throws<ArgumentNullException>(() => m_inputEventArgs.AddArgument("name", (string)null));
        }

        private InputEventArgs m_inputEventArgs;
    }
}
