using System;
using System.Collections;
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
        /// <summary>
        /// The zero vector. Note that this vector is immutable.
        /// </summary>
        public static Vector<Basis> Zero { get; private set; } = new ImmutableVector<Basis>();

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
                this[basis] += scalar;
            }
        }
        #endregion

        /// <summary>
        /// Gets or sets the scalar component for a particular basis element.
        /// If the component has not been set, it defaults to zero.
        /// </summary>
        /// <param name="basis">A basis object.</param>
        /// <returns>The scalar component for a particular basis element.</returns>
        public virtual double this[Basis basis]
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


        #region Addition and scaling methods
        /// <summary>
        /// Adds two vectors together.
        /// </summary>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <returns>The sum of the two vectors.</returns>
        public static Vector<Basis> operator +(Vector<Basis> lhs, Vector<Basis> rhs)
        {
            var res = new Vector<Basis>();
            foreach (var basis in lhs.bag.Keys.Union(rhs.bag.Keys))
            {
                res[basis] = lhs[basis] + rhs[basis];
            }
            return res;
        }

        /// <summary>
        /// Scales a vector with a scalar factor.
        /// </summary>
        /// <param name="factor">The scalar factor.</param>
        /// <param name="vector">The vector to be scaled.</param>
        /// <returns>A scaled vector.</returns>
        public static Vector<Basis> operator *(double factor, Vector<Basis> vector)
        {
            var res = new Vector<Basis>();
            foreach (var basis in vector.bag.Keys)
            {
                var scalar = vector[basis];
                if (scalar != 0)
                    res[basis] = factor * scalar;
            }
            return res;
        }

        /// <summary>
        /// Scales a vector with an inverse scalar factor.
        /// </summary>
        /// <param name="vector">The vector to be scaled.</param>
        /// <param name="divisor">The scalar divisor.</param>
        /// <returns>A scaled vector.</returns>
        public static Vector<Basis> operator /(Vector<Basis> vector, double divisor)
            => 1 / divisor * vector;

        /// <summary>
        /// Subtracts from a vector another vector.
        /// </summary>
        /// <remarks>
        /// Could be implemented more efficiently by mirroring the implementation
        /// of addition, instead of combining addition and scaling.
        /// </remarks>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static Vector<Basis> operator -(Vector<Basis> lhs, Vector<Basis> rhs)
            => lhs + -1 * rhs;
        #endregion

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

        
        /// <summary>
        /// Enumeration of this vector's components.
        /// </summary>
        public IEnumerable<VectorComponent<Basis>> Components
        {
            get => bag.Select(kv => new VectorComponent<Basis>(kv.Value, kv.Key));
        }

        /// <summary>
        /// Computes the <paramref name="p"/>-norm.
        /// 
        /// For <paramref name="p"/> = 1, this computes the Manhattan distance from 0 to this vector.
        /// For <paramref name="2"/> = 1, this computes the Euclidean length of this vector.
        /// </summary>
        /// <param name="p">Which norm to compute.</param>
        /// <returns>The <paramref name="p"/>-norm of this vector.</returns>
        public double Norm(int p = 2)
            => p >= 1
            ? Math.Pow(
                Components.Aggregate(0.0, (acc, component) => acc + Math.Pow(Math.Abs(component.Scalar), p)), 
                1.0 / p)
            : throw new ArgumentException("The p-norm is only defined for positive values of p");

        /// <summary>
        /// Projects each basis object of this vector into a new form.
        /// </summary>
        /// <typeparam name="B2">The basis object type of the value returned by the <paramref name="selector"/>.</typeparam>
        /// <param name="selector">A transform function to apply to each basis object.</param>
        /// <returns>A <see cref="Vector{Basis}"/> whose basis objects are the result of invoking the transform function on each original basis object.</returns>
        public Vector<B2> Select<B2>(Func<Basis,B2> selector)
        {
            var res = new Vector<B2>();
            foreach (var scalarObject in Components)
            {
                var obj2 = selector(scalarObject.Object);
                res[obj2] += scalarObject.Scalar;
            }
            return res;
        }

        /// <summary>
        /// Projects each basis object of this vector to a <see cref="Vector{Basis}"/> 
        /// and flattens the resulting nested vector into an unnested vector.
        /// That is, it lifts a function, which maps a basis vector to another vector,
        /// to a linear function from vectors to vectors and applies this linear function
        /// to this vector.
        /// </summary>
        /// <typeparam name="B2">The basis object type of the vector returned by the <paramref name="selector"/>.</typeparam>
        /// <param name="selector">A transform function to apply to each basis object.</param>
        /// <returns>A <see cref="Vector{Basis}"/> which is the result of the lifted <paramref name="selector"/> function.</returns>
        public Vector<B2> SelectMany<B2>(Func<Basis, Vector<B2>> selector)
            => Select(selector).Flatten();

        /// <summary>
        /// Projects each basis object of this vector to a <see cref="Vector{Basis}"/> 
        /// and flattens the resulting nested vector into an unnested vector.
        /// It differs from <see cref="SelectMany{B2}(Func{Basis, Vector{B2}})"/>
        /// in that it uses two functions for projecting each basis object.
        /// </summary>
        /// <typeparam name="B2">The type of the intermedia vector produced by <paramref name="vectorSelector"/>.</typeparam>
        /// <typeparam name="B3">The basis object type of value returned by the <paramref name="resultSelector"/>.</typeparam>
        /// <param name="vectorSelector">A transform function to apply to each basis object for creating an intermediate vector.</param>
        /// <param name="resultSelector">A transform function to apply to each basis object in the intermedia vector.</param>
        /// <returns>
        /// A <see cref="Vector{Basis}"/> which is the result of lifting the combination of <paramref name="vectorSelector"/> 
        /// and <paramref name="resultSelector"/> function.
        /// </returns>
        public Vector<B3> SelectMany<B2, B3>(Func<Basis, Vector<B2>> vectorSelector, Func<Basis, B2, B3> resultSelector)
            => SelectMany(basis => vectorSelector(basis).Select(basis2 => resultSelector(basis, basis2)));


        /// <summary>
        /// Filters a linear combination of basis vectors based on a predicate on the bases.
        /// </summary>
        /// <param name="predicate">A function to test each basis object for a condition.</param>
        /// <returns>A <see cref="Vector{Basis}"/> that retains the linear combination of basis vectors that satisfy the condition.</returns>
        public Vector<Basis> Where(Func<Basis, bool> predicate)
            => SelectMany(basis => predicate(basis) ? new Vector<Basis>(basis) : Zero);
    }


    /// <summary>
    /// Provides a set of <code>static</code> methods for <see cref="Vector{Basis}"/> objects.
    /// </summary>
    public static class Vector
    {
        /// <summary>
        /// Flattens a nested vector of vector objects.
        /// </summary>
        /// <remarks>
        /// This particular function is also known as the monadic "join".
        /// </remarks>
        /// <typeparam name="T">The inner basis of the vector space.</typeparam>
        /// <param name="vectorOfVectors">A nested vector of vector objects.</param>
        /// <returns>A flattened vector.</returns>
        public static Vector<T> Flatten<T>(this Vector<Vector<T>> vectorOfVectors)
        {
            var res = new Vector<T>();
            foreach (var scalarVector in vectorOfVectors.Components)
            {
                foreach (var scalarObject in scalarVector.Object.Components)
                {
                    res[scalarObject.Object] += scalarVector.Scalar * scalarObject.Scalar;
                }
            }
            return res;
        }
    }
}
