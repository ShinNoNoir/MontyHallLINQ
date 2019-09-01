using System;
using System.Collections.Generic;
using Xunit;

namespace VectorSpace.Tests
{
    public class ZeroImmutabilityTest
    {
        [Fact]
        public void ImmutabilityTest()
        {
            var exception = Record.Exception(() =>
            {
                var z = Vector<bool>.Zero;
                z[false] = 1;
                z[true] = 1;
            });
            Assert.NotNull(exception);
        }
    }
}
