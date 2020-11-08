using System;
using Xunit;

namespace TrxTests.XUnit.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test_Passes()
        {
        }

        [Fact]
        public void Test_Fails()
        {
            throw new System.Exception ("Ahhhhhhh!!!");
        }
    }
}
