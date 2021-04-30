using System;
using Xunit;

namespace TrxTests.XUnit.AllPass.Tests
{
    public class ParamTests
    {
        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 2)]
        public void Test_Passes_Easily(bool b, int i)
        {

        }
    }
}
