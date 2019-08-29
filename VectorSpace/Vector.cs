using System;
using System.Collections.Generic;
using System.Linq;

namespace VectorSpace
{
    /// <summary>
    /// A vector, i.e., an element of a particular vector space 
    /// with a particular <typeparamref name="Basis"/>.
    /// </summary>
    /// <remarks>
    /// The scalars of the particular vector space are elements of
    /// the field of <see cref="double"/>s. Theoretically, this class
    /// could be made more generic by introducing a type parameter for
    /// the field of scalars. Unfortunately, constraining the type for 
    /// this parameter to be numeric or field-like is cumbersome in C#.
    /// </remarks>
    /// <typeparam name="Basis">The basis of the vector space.</typeparam>
    public class Vector<Basis>
    {
        #region Constructors
        /// <summary>
        /// The backing datastructure for a vector.
        /// </summary>
        /// <remarks>
        /// The field is called bag, since a vector is basically just a 
        /// bag of basis objects.
        /// </remarks>
        private Dictionary<Basis, double> bag = new Dictionary<Basis, double>();

        /// <summary>
        /// Constructs a zero vector.
        /// </summary>
        public Vector() { }

        /// <summary>
        /// Constructs a basis vector.
        /// </summary>
        /// <param name="basis">A basis object.</param>
        public Vector(Basis basis)
        {
            bag[basis] = 1.0;
        }

        /// <summary>
        /// Constructs a vector consisting of a linear combination of basis vectors.
        /// </summary>
        /// <param name="scalarBasisPairs">A list of scalar-basis object tuples.</param>
        public Vector(params (double, Basis)[] scalarBasisPairs)
        {
            foreach ((var scalar, var basis) in scalarBasisPairs)
            {
                bag[basis] = scalar;
            }
        }
        #endregion

        /// <summary>
        /// Gets or sets the scalar component for a particular basis element.
        /// If the component has not been set, it defaults to zero.
        /// </summary>
        /// <param name="basis">A basis object.</param>
        /// <returns>The scalar component for a particular basis element.</returns>
        public double this[Basis basis]
        {
            get { return bag.TryGetValue(basis, out double value) ? value : 0.0; }
            set { bag[basis] = value; }
        }

        /// <summary>
        /// Returns a string representation of a vector.
        /// </summary>
        /// <returns>String representation of a vector.</returns>
        public override string ToString()
            => $"{{{string.Join(", ", from entry in bag select $"{ entry.Key}: { entry.Value}")}}}";
        
                
        #region Equality methods
        /// <summary>
        /// Determines whether two vector objects are equal. See <see cref="Equals(Vector{Basis})"/>.
        /// </summary>
        /// <param name="other">The other vector object.</param>
        /// <returns>True if the other object equals this vector.</returns>
        public override bool Equals(object other)
            => this.Equals(other as Vector<Basis>);


        /// <summary>
        /// Determines whether two vector objects are equal.
        /// 
        /// Two vectors are equal if their components for each of the basis elements are equal.
        /// </summary>
        /// <param name="other">The other vector object</param>
        /// <returns>True if and only if the two vectors' components for each basis element agree.</returns>
        public bool Equals(Vector<Basis> other)
        {
            if (object.ReferenceEquals(other, null))
                return false;

            if (Object.ReferenceEquals(this, other))
                return true;

            if (this.GetType() != other.GetType())
                return false;

            var keys = new HashSet<Basis>(bag.Keys.Concat(other.bag.Keys));
            return keys.All(key => this[key] == other[key]);
        }

        /// <summary>
        /// Returns the hash code of a vector.
        /// </summary>
        /// <returns>The hash code of this vector.</returns>
        public override int GetHashCode()
        {
            int hash = 0;
            foreach (var entry in bag)
            {
                if (entry.Value != 0.0)
                    hash ^= entry.Key.GetHashCode() ^ entry.Value.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// The equality operator for vectors. See <see cref="Equals(Vector{Basis})"/>.
        /// </summary>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <returns>True if and only if the two vectors are equal.</returns>
        public static bool operator ==(Vector<Basis> lhs, Vector<Basis> rhs)
            => object.ReferenceEquals(lhs, null)
                ? Object.ReferenceEquals(rhs, null)
                : lhs.Equals(rhs);

        /// <summary>
        /// The inequality operator. See <see cref="operator ==(Vector{Basis}, Vector{Basis})"/>.
        /// </summary>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <returns>True if and only if the two vectors are not equal.</returns>
        public static bool operator !=(Vector<Basis> lhs, Vector<Basis> rhs)
            => !(lhs == rhs);
        #endregion
    }
}
