using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace VectorSpace.Tests
{
    public class VectorArithmeticTests
    {
        [Theory]
        [MemberData(nameof(GetVectorData))]
        public void ZeroTest<T>(Vector<T> v)
        {
            Assert.Equal(v + Vector<T>.Zero, v);
            Assert.Equal(0 * v, Vector<T>.Zero);
        }

        [Theory]
        [MemberData(nameof(GetVectorPairsData))]
        public void CommutativeAdditionTest<T>(Vector<T> u, Vector<T> v)
        {
            Assert.Equal(u + v, v + u);
        }

        [Theory]
        [MemberData(nameof(GetVectorTripletsData))]
        public void AssociativeAdditionTest<T>(Vector<T> u, Vector<T> v, Vector<T> w)
        {
            Assert.Equal((u + v) + w, u + (v + w));
        }

        [Theory]
        [MemberData(nameof(GetScalarScalarVectorData))]
        public void AssociativeScalingTest<T>(double a, double b, Vector<T> v)
        {
            Assert.Equal(a * (b * v), (a * b) * v);
        }

        [Theory]
        [MemberData(nameof(GetScalarVectorVectorData))]
        public void DistributiveScalingTest<T>(double a, Vector<T> u, Vector<T> v)
        {
            Assert.Equal(a * u + a * v, a * (u + v));
        }


        [Theory]
        [MemberData(nameof(GetVectorPairsData))]
        public void AdditionViaFlattenTest<T>(Vector<T> u, Vector<T> v)
        {
            Assert.Equal(
                u + v,
                new Vector<Vector<T>>((1, u), (1, v)).Flatten()
            );
        }

        [Theory]
        [MemberData(nameof(GetScalarVectorData))]
        public void ScalingViaFlattenTest<T>(double a, Vector<T> v)
        {
            Assert.Equal(
                a * v,
                new Vector<Vector<T>>((a, v)).Flatten()
            );
        }



        public static IEnumerable<object> SampleVectors
        { get; private set; } =
            new List<object>
            {
                new Vector<bool>(),
                new Vector<bool>(false),
                new Vector<bool>(true),
                new Vector<bool>((1.0, false), (1.0, true))
            };

        public static IEnumerable<object[]> GetVectorData()
            => from v in SampleVectors
               select new object[] { v };

        public static IEnumerable<object[]> GetVectorPairsData()
            => from u in SampleVectors
               from v in SampleVectors
               select new object[] { u, v };

        public static IEnumerable<object[]> GetVectorTripletsData()
            => from u in SampleVectors
               from v in SampleVectors
               from w in SampleVectors
               select new object[] { u, v, w };

        public static IEnumerable<object> SampleScalars
        { get; private set; } = new List<object> { 0.0, 1.0, -1.0, 5.0 };

        public static IEnumerable<object[]> GetScalarVectorData()
            => from a in SampleScalars
               from v in SampleVectors
               select new object[] { a, v };

        public static IEnumerable<object[]> GetScalarScalarVectorData()
            => from a in SampleScalars
               from b in SampleScalars
               from v in SampleVectors
               select new object[] { a, b, v };

        public static IEnumerable<object[]> GetScalarVectorVectorData()
            => from a in SampleScalars
               from u in SampleVectors
               from v in SampleVectors
               select new object[] { a, u, v };
    }
}
