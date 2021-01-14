using NUnit.Framework;

namespace TrxTests.NUnit.AllPass.Tests
{
    public class Tests
    {
        [Test]
        public void Test_DoesSomething_AndPasses()
        {
            Assert.Pass();
        }

        [Test]
        public void Test_DoesSomething_AndAlsoPasses()
        {
            Assert.Pass();
        }
    }
}