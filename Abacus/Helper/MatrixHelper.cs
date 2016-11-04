using System;
using Abacus.Interface;

namespace Abacus.Helper
{
    public class MatrixHelper
    {
        public static double PointProject(Matrix3 orientation, Vector3 point)
        {
            return orientation[0, 0]*point[0] + orientation[1, 0]*point[1] + orientation[2, 0]*point[2];
        }

        public static Vector3 Vector3ChangeBasis(Matrix3 newBasis, Vector3 oldVect)
        {
            return new Vector3(
                oldVect[0]*newBasis[0, 0] + oldVect[1]*newBasis[1, 0] + oldVect[2]*newBasis[2, 0],
                oldVect[0]*newBasis[0, 1] + oldVect[1]*newBasis[1, 1] + oldVect[2]*newBasis[2, 1],
                oldVect[0]*newBasis[0, 2] + oldVect[1]*newBasis[1, 2] + oldVect[2]*newBasis[2, 2]
                );
        }

        public static Matrix3 Matrix3ChangeBasis(Matrix3 newBasis, Matrix3 oldBasis)
        {
            var m = new Matrix3();
            m[0, 0] = (newBasis[0, 0]*oldBasis[0, 0] +
                       newBasis[1, 0]*oldBasis[1, 0] +
                       newBasis[2, 0]*oldBasis[2, 0]);
            m[1, 0] = (newBasis[0, 1]*oldBasis[0, 0] +
                       newBasis[1, 1]*oldBasis[1, 0] +
                       newBasis[2, 1]*oldBasis[2, 0]);
            m[2, 0] = (newBasis[0, 2]*oldBasis[0, 0] +
                       newBasis[1, 2]*oldBasis[1, 0] +
                       newBasis[2, 2]*oldBasis[2, 0]);
            m[0, 1] = (newBasis[0, 0]*oldBasis[0, 1] +
                       newBasis[1, 0]*oldBasis[1, 1] +
                       newBasis[2, 0]*oldBasis[2, 1]);
            m[1, 1] = (newBasis[0, 1]*oldBasis[0, 1] +
                       newBasis[1, 1]*oldBasis[1, 1] +
                       newBasis[2, 1]*oldBasis[2, 1]);
            m[2, 1] = (newBasis[0, 2]*oldBasis[0, 1] +
                       newBasis[1, 2]*oldBasis[1, 1] +
                       newBasis[2, 2]*oldBasis[2, 1]);
            m[0, 2] = (newBasis[0, 0]*oldBasis[0, 2] +
                       newBasis[1, 0]*oldBasis[1, 2] +
                       newBasis[2, 0]*oldBasis[2, 2]);
            m[1, 2] = (newBasis[0, 1]*oldBasis[0, 2] +
                       newBasis[1, 1]*oldBasis[1, 2] +
                       newBasis[2, 1]*oldBasis[2, 2]);
            m[2, 2] = (newBasis[0, 2]*oldBasis[0, 2] +
                       newBasis[1, 2]*oldBasis[1, 2] +
                       newBasis[2, 2]*oldBasis[2, 2]);
            return m;
        }

        /// <summary>
        ///     Basic matrix. Does not initialize any values
        /// </summary>
        public static double[,] Fill(int dim, double value)
        {
            var values = new double[dim, dim];
            for (int m = 0; m < dim; m++)
            {
                for (int n = 0; n < dim; n++)
                {
                    values[m, n] = value;
                }
            }
            return values;
        }

        /// <summary>
        ///     Basic matrix. Does not initialize any values
        /// </summary>
        public static T Fill<T>(int dim, double value) where T : IMatrix
        {
            var values = new double[dim, dim];
            for (int m = 0; m < dim; m++)
            {
                for (int n = 0; n < dim; n++)
                {
                    values[m, n] = value;
                }
            }
            return (T) Activator.CreateInstance(typeof (T), values);
        }

        /// <summary>
        ///     A static identity matrix creator. Returns an identity matrix.
        /// </summary>
        public static double[,] Identity(int dim)
        {
            var values = new double[dim, dim];
            for (int m = 0; m < dim; m++)
            {
                for (int n = 0; n < dim; n++)
                {
                    values[m, n] = m != n ? 0 : 1;
                }
            }
            return values;
        }

        /// <summary>
        ///     Returns a strongly typed identity matrix of type T
        /// </summary>
        /// <typeparam name="T">Matrix3 or Matrix4</typeparam>
        /// <returns>a strongly typed identity matrix of type T</returns>
        public static T Identity<T>() where T : IMatrix
        {
            double[,] values = null;
            if (typeof (T) == typeof (Matrix4))
            {
                values = Identity(4);
            }
            if (typeof (T) == typeof (Matrix3))
            {
                values = Identity(3);
            }
            return (T) Activator.CreateInstance(typeof (T), values);
        }
    }
}