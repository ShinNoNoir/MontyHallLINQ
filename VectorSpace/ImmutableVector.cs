using System;
using System.Collections.Generic;
using System.Text;

namespace VectorSpace
{
    /// <summary>
    /// An internal class which disallows its internal state to be modified.
    /// </summary>
    /// <remarks>
    /// Note that this class violates the Liskov Substitution Principle.
    /// In a way, it would be cleaner to have <see cref="Vector{Basis}"/>
    /// and <see cref="ImmutableVector{Basis}"/> to be siblings and have
    /// them share a common base class.
    /// </remarks>
    /// <typeparam name="Basis">The basis of the vector space.</typeparam>
    internal class ImmutableVector<Basis> : Vector<Basis>
    {
        public override double this[Basis basis]
        {
            get { return base[basis]; }
            set { throw new InvalidOperationException("An immutable vector cannot be mutated."); }
        }
    }
}
