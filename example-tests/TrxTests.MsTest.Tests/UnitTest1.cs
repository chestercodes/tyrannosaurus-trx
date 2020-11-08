using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrxTests.MsTest.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_Passes()
        {
        }

        [TestMethod]
        public void Test_Fails()
        {
            throw new System.Exception ("Ahhhhhhh!!!");
        }
    }
}
