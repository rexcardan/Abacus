using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.Exceptions;
using Abacus.Interface;

namespace Abacus
{
    public static class VectorExtensions
    {
        /// <summary>
        ///     Returns the vectors as rows in a matrix (double[,])
        /// </summary>
        /// <param name="vectors">m vectors to acts as rows</param>
        /// <returns>the m x 3 matrix representation </returns>
        public static double[,] ToRowMatrix(this IEnumerable<IVector> vectors)
        {
            IVector[] vArray = vectors.ToArray();
            IVector sample = vectors.First();
            var matrix = new double[vectors.Count(), sample.Length];
            for (int i = 0; i < vArray.Length; i++)
            {
                IVector v = vArray[i];
                for (int j = 0; j < v.Length; j++)
                {
                    matrix[i, j] = v[j];
                }
            }
            return matrix;
        }

        public static double[,] ToColumnMatrix(this IVector vect)
        {
            double[] vArray = vect.Values;
            var matrix = new double[vArray.Length, 1];
            for (int i = 0; i < vArray.Length; i++)
            {
                matrix[i, 0] = vArray[i];
            }
            return matrix;
        }

        public static double[,] ToRowMatrix(this IVector vect)
        {
            return vect.ToColumnMatrix().Transpose();
        }

        /// <summary>
        ///     Returns the vectors as rows in a matrix (double[,])
        /// </summary>
        /// <param name="vectors">m vectors to acts as rows</param>
        /// <returns>the m x 3 matrix representation </returns>
        public static List<Vector4> ToVector4s(this IEnumerable<Vector3> vectors)
        {
            return vectors.Select(v => v.Homogeneous).ToList();
        }

        /// <summary>
        ///     Returns the vectors as rows in a matrix (double[,])
        /// </summary>
        /// <param name="vectors">m vectors to acts as rows</param>
        /// <returns>the m x 3 matrix representation </returns>
        public static List<Vector3> ToVector3s(this IEnumerable<Vector4> vectors)
        {
            Vector4[] vArray = vectors.ToArray();
            var list = new List<Vector3>();
            for (int i = 0; i < vArray.Length; i++)
            {
                Vector4 v = vArray[i];
                list.Add(new Vector3(v.X, v.Y, v.Z));
            }
            return list;
        }

        /// <summary>
        ///     Computes the centroid point of the point cloud
        /// </summary>
        /// <param name="points">the points or vectors in the point cloud</param>
        /// <returns>the centroid point of the cloud</returns>
        public static Vector3 GetCentroid(this IEnumerable<Vector3> points)
        {
            Vector3 result = Vector3.Zeroes;
            foreach (Vector3 point in points)
            {
                result += point;
            }
            return result/points.Count();
        }

        /// <summary>
        ///     Repeats a vector a given amount of times as rows in a matrix represented by a jagged array
        /// </summary>
        /// <param name="v">the vector to repeat</param>
        /// <param name="numRows">the number of times to repeat (the number of rows)</param>
        /// <returns>a jagged array representing the underlying matrix</returns>
        public static double[,] RepMat(this IVector v, int numRows)
        {
            var matrix = new double[0, v.Length];
            for (int i = 0; i < numRows; i++)
            {
                matrix = matrix.InsertRow(v, i);
            }
            return matrix;
        }

        public static double[] ElementWiseOperate(this double[] v1, double[] v2, Func<double, double, double> op)
        {
            if (v1.Length != v2.Length)
            {
                throw new InvalidSizeException(v1.Length, v2.Length);
            }
            var result = new List<double>();
            for (int i = 0; i < v1.Length; i++)
            {
                result.Add(op(v1[i], v2[i]));
            }
            return result.ToArray();
        }

        public static double[] Add(this IVector v1, IVector v2)
        {
            return ElementWiseOperate(v1.Values, v2.Values, (v1v, v2v) => { return v1v + v2v; });
        }

        public static double[] Add(this double[] v1, IVector v2)
        {
            return ElementWiseOperate(v1, v2.Values, (v1v, v2v) => { return v1v + v2v; });
        }

        public static double[] Subtract(this IVector v1, IVector v2)
        {
            return ElementWiseOperate(v1.Values, v2.Values, (v1v, v2v) => { return v1v - v2v; });
        }

        public static double[] Subtract(this double[] v1, IVector v2)
        {
            return ElementWiseOperate(v1, v2.Values, (v1v, v2v) => { return v1v - v2v; });
        }

        public static double[] Multiply(this IVector v1, IVector v2)
        {
            return ElementWiseOperate(v1.Values, v2.Values, (v1v, v2v) => { return v1v*v2v; });
        }

        public static double[] Multiply(this double[] v1, IVector v2)
        {
            return ElementWiseOperate(v1, v2.Values, (v1v, v2v) => { return v1v*v2v; });
        }

        public static double[] Divide(this IVector v1, IVector v2)
        {
            return ElementWiseOperate(v1.Values, v2.Values, (v1v, v2v) => { return v1v/v2v; });
        }

        public static double[] Divide(this double[] v1, IVector v2)
        {
            return ElementWiseOperate(v1, v2.Values, (v1v, v2v) => { return v1v/v2v; });
        }
    }
}