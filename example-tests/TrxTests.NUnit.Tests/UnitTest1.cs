using NUnit.Framework;

namespace TrxTests.NUnit.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_Passes()
        {
        }

        [Test]
        public void Test_Fails()
        {
            throw new System.Exception ("Ahhhhhhh!!!");
        }
    }
}