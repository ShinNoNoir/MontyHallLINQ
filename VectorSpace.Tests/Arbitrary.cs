using System;
using System.Collections.Generic;
using System.Text;

namespace VectorSpace.Tests
{
    public static class Arbitrary
    {
        private static Random Rand = new Random();
        public static bool GetRandomBool() => Rand.Next(2) == 0 ? false : true;
        public static double GetRandomScalar() => Rand.Next(-2, 10);


        public static Vector<bool> GetRandomVector()
        {
            var res = new Vector<bool>();
            var numComponents = Rand.Next(5);
            for (var i = 0; i != numComponents; ++i)
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

    }
}
