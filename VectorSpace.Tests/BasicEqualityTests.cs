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
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void HashCodeTest<T>(Vector<T> u, Vector<T> v)
        {
            if (u != null && v != null)
                Assert.True(u.GetHashCode() == v.GetHashCode(), $"{u} and {v} should have same hash code");
        }

        public static IEnumerable<object[]> GetData()
            => new List<object[]>
            {
                new object[]{ null, null },
                new object[]{ new Vector<bool>(true), new Vector<bool>(true) },
                new object[]{ new Vector<bool>((0, false), (1.0, true)), new Vector<bool>(true) },
                new object[]{ new Vector<bool>((1.0, false), (0.0, true)), new Vector<bool>(false) }
            };
    }
}
