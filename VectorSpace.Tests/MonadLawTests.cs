using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace VectorSpace.Tests
{
    public class MonadLawTests
    {
        [Theory]
        [MemberData(nameof(GetRandomNestedNestedVectorData))]
        public void AssociativeFlattenTest<T>(Vector<Vector<Vector<T>>> v)
        {
            Assert.Equal(
                v.Flatten().Flatten(),
                v.Select(u => u.Flatten()).Flatten()
            );
        }

        [Theory]
        [MemberData(nameof(GetRandomVectorData))]
        public void IdentityFlattenTest<T>(Vector<T> v)
        {
            Assert.Equal(
                v.Select(a => new Vector<T>(a)).Flatten(),
                new Vector<Vector<T>>(v).Flatten()
            );
        }

        [Theory]
        [MemberData(nameof(GetRandomVectorData))]
        public void ZeroSelectManyTest<T>(Vector<T> v)
        {
            // The first part of this test requires any arbitrary T -> V<S> function:
            Func<T, Vector<T>> anyFunction = _ => v;

            // 0.SelectMany(f) = 0
            Assert.Equal(
                Vector<T>.Zero.SelectMany(anyFunction),
                Vector<T>.Zero
            );

            // v.SelectMany(_ => 0) = 0
            Assert.Equal(
                v.SelectMany(_ => Vector<T>.Zero),
                Vector<T>.Zero
            );
        }


        [Theory]
        [MemberData(nameof(GetRandomVectorData))]
        public void WhereIdentityTest<T>(Vector<T> v)
        {
            Assert.Equal(
                v.Where(x => true),
                v
            );
        }

        [Theory]
        [MemberData(nameof(GetRandomVectorData))]
        public void WhereZeroTest<T>(Vector<T> v)
        {
            Assert.Equal(
                v.Where(x => false),
                Vector<T>.Zero
            );
        }



        public static IEnumerable<object[]> GetRandomVectorData()
            => Enumerable.Range(0, 100).Select(_ => new object[] { Arbitrary.GetRandomVector() });

        public static IEnumerable<object[]> GetRandomNestedNestedVectorData()
            => Enumerable.Range(0, 100).Select(_ => new object[] { Arbitrary.GetRandomNestedNestedVector() });

    }
}
