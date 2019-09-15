using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace VectorSpace.Tests
{
    public class NormTests
    {
        private const double EPSILON = 0.00001;


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

        [Theory]
        [MemberData(nameof(GetRandomVectorData))]
        public void NormalizedTest<T>(Vector<T> v)
        {
            // Skip zero vectors
            if (v.Norm(1) == 0)
                return;

            for (int p = 1; p <= 3; ++p)
            {
                var normalizedV = v.Normalized(p);
                Assert.True(
                    Math.Abs(v.Normalized(p).Norm(p) - 1) <= EPSILON,
                    $"{v} {p}-normalized should have a {p}-norm of 1, but was {v.Normalized(p).Norm(p)} instead; Unnormalized norm is {v.Norm(p)}."
                );
            }
        }


        public static IEnumerable<object[]> GetRandomVectorData()
            => Enumerable.Range(0, 100).Select(_ => new object[] { Arbitrary.GetRandomVector() });

    }
}
