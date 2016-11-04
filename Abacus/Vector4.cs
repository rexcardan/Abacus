using System;
using Abacus.Exceptions;
using Abacus.Interface;

namespace Abacus
{
    public class Vector4 : IVector
    {
        private double[] values;

        #region CONSTRUCTORS

        public Vector4(double[] values)
        {
            if (values.Length != 4)
            {
                throw new Exception("Values must be of length 4!");
            }
            this.values = values;
        }

        public Vector4(double x, double y, double z, double w)
        {
            values = new[] {x, y, z, w};
        }

        #endregion

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
                throw new IndexOutOfRangeException("Vector 4 array only has length of 4!");
            }
            set
            {
                if (index < values.Length)
                    values[index] = value;
                else
                {
                    throw new IndexOutOfRangeException("Vector 4 array only has length of 4!");
                }
            }
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        ///     A static empty value vector creator. Returns a vector4 containing only NaN value elements.
        /// </summary>
        public static Vector4 Empty
        {
            get { return new Vector4(double.NaN, double.NaN, double.NaN, double.NaN); }
        }

        /// <summary>
        ///     A static zero value vector creator. Returns a vector4 containing only zero value elements.
        /// </summary>
        public static Vector4 Zeroes
        {
            get { return new Vector4(0, 0, 0, 0); }
        }

        /// <summary>
        ///     A static infinite value vector creator. Returns a vector4 containing positive infinite value elements.
        /// </summary>
        public static Vector4 Infinite
        {
            get
            {
                return new Vector4(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity,
                    double.PositiveInfinity);
            }
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

        public double Z
        {
            get { return this[2]; }
            set { this[2] = value; }
        }

        public double W
        {
            get { return this[3]; }
            set { this[3] = value; }
        }

        public int Length
        {
            get { return 4; }
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

        #region METHODS

        /// <summary>
        ///     Creates a copy of this vector
        /// </summary>
        /// <returns></returns>
        public Vector4 Copy()
        {
            return new Vector4(this[0], this[1], this[2], this[3]);
        }

        /// <summary>
        ///     Finds the magnitude of this vector
        /// </summary>
        /// <returns>a double representing the magnitude of the vector</returns>
        public double Norm()
        {
            return System.Math.Sqrt(this*this);
        }

        public float DistanceTo(Vector4 v)
        {
            return
                (float)
                    System.Math.Sqrt(System.Math.Pow((v[0] - this[0]), 2) + System.Math.Pow((v[1] - this[1]), 2) +
                                     System.Math.Pow((v[2] - this[2]), 2));
        }

        public double[] ToArray()
        {
            return values;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", values[0], values[1], values[2], values[3]);
        }

        #endregion

        #region OPERATORS

        public static Vector4 operator +(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1[0] + v2[0], v1[1] + v2[1], v1[2] + v2[2], v1[3] + v2[3]);
        }

        public static Vector4 operator -(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1[0] - v2[0], v1[1] - v2[1], v1[2] - v2[2], v1[3] - v2[3]);
        }

        public static Vector4 operator *(Vector4 v1, double s)
        {
            return new Vector4(v1[0]*s, v1[1]*s, v1[2]*s, v1[3]*s);
        }

        /// <summary>
        ///     Computes the scalar (dot) product of two vectors
        /// </summary>
        /// <param name="v1">the first vector</param>
        /// <param name="v2">the second vector</param>
        /// <returns>The scalar product of two vectors</returns>
        public static double operator *(Vector4 v1, Vector4 v2)
        {
            return v1[0]*v2[0] + v1[1]*v2[1] + v1[2]*v2[2] + v1[3]*v2[3];
        }

        public static Vector4 operator /(Vector4 v1, double s)
        {
            return new Vector4(v1[0]/s, v1[1]/s, v1[2]/s, v1[3]/s);
        }

        public static bool operator ==(Vector4 v1, Vector4 v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Vector4 v1, Vector4 v2)
        {
            return !v1.Equals(v2);
        }

        #endregion

        #region COMPARATORS

        public override bool Equals(object obj)
        {
            if (!(obj is Vector4))
            {
                return false;
            }
            var v = (Vector4) obj;
            return ((v.X == X) && (v.Y == Y) && (v.Z == Z) && (v.W == W));
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        #endregion
    }
}