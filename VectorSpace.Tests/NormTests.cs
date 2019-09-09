using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace VectorSpace.Tests
{
    public class NormTests
    {
        [Theory]
        [MemberData(nameof(GetRandomVectorData))]
        public void Norm1Test<T>(Vector<T> v)
        {
            Assert.Equal(
                v.Norm(1),
                v.Select(_ => "norm")["norm"]
            );
        }

        [Theory]
        [MemberData(nameof(GetRandomVectorData))]
        public void Norm2Test<T>(Vector<T> v)
        {
            Assert.Equal(
                v.Norm(2),
                Math.Pow(
                    v.SelectMany(b => v.Where(b2 => b.Equals(b2)))
                        .Select(_ => "normSquared")["normSquared"],
                    0.5)
            );
        }


        public static IEnumerable<object[]> GetRandomVectorData()
            => Enumerable.Range(0, 100).Select(_ => new object[] { Arbitrary.GetRandomVector() });

    }
}
