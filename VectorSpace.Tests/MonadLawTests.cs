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



        private static Random Rand = new Random();
        public static bool GetRandomBool() => Rand.Next(2) == 0 ? false : true;
        public static double GetRandomScalar() => Rand.Next(-2, 10);
        
        public static Vector<bool> GetRandomVector()
        {
            var res = new Vector<bool>();
            var numComponents = Rand.Next(5);
            for (var i=0; i != numComponents; ++i)
            {
                res[GetRandomBool()] = GetRandomScalar();
            }
            return res;
        }

        public static Vector<Vector<bool>> GetRandomNestedVector()
        {
            var res = new Vector<Vector<bool>>();
            var numComponents = Rand.Next(5);
            for (var i = 0; i != numComponents; ++i)
            {
                res[GetRandomVector()] = GetRandomScalar();
            }
            return res;
        }

        public static Vector<Vector<Vector<bool>>> GetRandomNestedNestedVector()
        {
            var res = new Vector<Vector<Vector<bool>>>();
            var numComponents = Rand.Next(5);
            for (var i = 0; i != numComponents; ++i)
            {
                res[GetRandomNestedVector()] = GetRandomScalar();
            }
            return res;
        }

        public static IEnumerable<object[]> GetRandomVectorData()
            => Enumerable.Range(0, 100).Select(_ => new object[] { GetRandomVector() });

        public static IEnumerable<object[]> GetRandomNestedNestedVectorData()
            => Enumerable.Range(0, 100).Select(_ => new object[] { GetRandomNestedNestedVector() });

    }
}
