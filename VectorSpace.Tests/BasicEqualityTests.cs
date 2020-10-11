using System;
using System.Collections.Generic;
using Xunit;

namespace VectorSpace.Tests
{
    public class BasicEqualityTests
    {
        [Theory]
        [MemberData(nameof(GetData))]
        public void EqualityTest<T>(Vector<T> u, Vector<T> v)
        {
            Assert.Equal(u, v);
            Assert.Equal(v, u);
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void HashCodeTest<T>(Vector<T> u, Vector<T> v)
        {
            if (u != null && v != null)
                Assert.True(u.GetHashCode() == v.GetHashCode(), $"{u} and {v} should have same hash code");
        }

        public static TheoryData<Vector<bool>, Vector<bool>> GetData
            => new()
            {
                { new Vector<bool>((1.0, false), (0.0, true)), new Vector<bool>(false) },
                { null, null },
                { new Vector<bool>(), Vector<bool>.Zero},
                { new Vector<bool>(true), new Vector<bool>(true) },
                { new Vector<bool>((0, false), (1.0, true)), new Vector<bool>(true) },
            };
    }
}
