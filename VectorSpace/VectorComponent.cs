using System;
using System.Collections.Generic;
using System.Text;

namespace VectorSpace
{
    /// <summary>
    /// A scalar/object pair used for iterating over the components of a vector.
    /// </summary>
    /// <remarks>
    /// Basically a <see cref="KeyValuePair{TKey, TValue}"/>, but with different property names.
    /// </remarks>
    /// <typeparam name="Basis">The basis of the vector space.</typeparam>
    public struct VectorComponent<Basis>
    {
        /// <summary>
        /// Constructs a scalar/object pair.
        /// </summary>
        /// <param name="scalar">The scalar component.</param>
        /// <param name="basisObject">The basis object.</param>
        public VectorComponent(double scalar, Basis basisObject)
        {
            Scalar = scalar;
            Object = basisObject;
        }
        /// <summary>
        /// The scalar component.
        /// </summary>
        public double Scalar { get; private set; }
        /// <summary>
        /// The basis object.
        /// </summary>
        public Basis Object { get; private set; }
    }
}
