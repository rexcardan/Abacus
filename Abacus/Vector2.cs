using System;
using Abacus.Exceptions;
using Abacus.Interface;

namespace Abacus
{
    public class Vector2 : IVector
    {
        private double[] values;

        public Vector2(double x, double y)
        {
            values = new double[2] {x, y};
        }

        public double Norm()
        {
            return System.Math.Sqrt(this*this);
        }

        public float DistanceTo(Vector2 v)
        {
            return (float) System.Math.Sqrt(System.Math.Pow((v[0] - this[0]), 2) + System.Math.Pow((v[1] - this[1]), 2));
        }

        public double[] ToArray()
        {
            return values;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", values[0], values[1]);
        }

        #region ACCESSORS

        /// <summary>
        ///     Allows the vector class elements to be accessed by index
        /// </summary>
        /// <param name="index">the index of the element to return</param>
        /// <returns>the element at the specified index</returns>
        public double this[long index]
        {
            get
            {
                if (index < values.Length)
                    return values[index];
                throw new IndexOutOfRangeException("Vector 2 array only has length of 2!");
            }
            set
            {
                if (index < values.Length)
                    values[index] = value;
                else
                {
                    throw new IndexOutOfRangeException("Vector 2 array only has length of 2!");
                }
            }
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        ///     A static zero value vector creator. Returns a vector2 containing only zero value elements.
        /// </summary>
        public static Vector2 Zeroes
        {
            get { return new Vector2(0, 0); }
        }

        /// <summary>
        ///     A static infinite value vector creator. Returns a vector2 containing positive infinite value elements.
        /// </summary>
        public static Vector2 Infinite
        {
            get { return new Vector2(double.PositiveInfinity, double.PositiveInfinity); }
        }

        public double X
        {
            get { return this[0]; }
            set { this[0] = value; }
        }

        public double Y
        {
            get { return this[1]; }
            set { this[1] = value; }
        }

        public Vector3 Homogeneous
        {
            get { return new Vector3(X, Y, 1); }
        }

        public int Length
        {
            get { return 2; }
        }

        public double[] Values
        {
            get { return values; }
            set
            {
                if (value.Length != Length)
                {
                    throw new InvalidSizeException(Length, values.Length);
                }
                values = value;
            }
        }

        #endregion

        #region OPERATORS

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1[0] + v2[0], v1[1] + v2[1]);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1[0] - v2[0], v1[1] - v2[1]);
        }

        public static Vector2 operator *(Vector2 v1, double s)
        {
            return new Vector2(v1[0]*s, v1[1]*s);
        }

        /// <summary>
        ///     Computes the scalar (dot) product of two vectors
        /// </summary>
        /// <param name="v1">the first vector</param>
        /// <param name="v2">the second vector</param>
        /// <returns>The scalar product of two vectors</returns>
        public static double operator *(Vector2 v1, Vector2 v2)
        {
            return v1[0]*v2[0] + v1[1]*v2[1];
        }

        public static Vector2 operator /(Vector2 v1, double s)
        {
            return new Vector2(v1[0]/s, v1[1]/s);
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !v1.Equals(v2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2)
            {
                var v = (Vector2) obj;
                return ReferenceEquals(v, null) ? false : ((v.X == X) && (v.Y == Y));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        #endregion
    }
}