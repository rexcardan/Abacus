using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.MatrixDecomp;

namespace Abacus.Helper
{
    public static class TransformHelper
    {
        /// <summary>
        ///     This creates a transformation matrix from left handed angle inputs of the rotation
        ///     on each axis (in radians)
        /// </summary>
        /// <param name="rotXRadians">the left handed (clockwise) angle of rotation from the x axis</param>
        /// <param name="rotYRadians">the left handed (clockwise) angle of rotation from the y axis</param>
        /// <param name="rotZRadians">the left handed (clockwise) angle of rotation from the z axis</param>
        /// <returns>the rotation matrix for performing transforms</returns>
        public static Matrix3 GetRotationMatrixFromRadians(double rotXRadians, double rotYRadians, double rotZRadians)
        {
            var m = new Matrix3();
            m[0, 0] = System.Math.Cos(rotYRadians)*System.Math.Cos(rotZRadians);
            m[0, 1] = -System.Math.Cos(rotXRadians)*System.Math.Sin(rotZRadians) +
                      System.Math.Sin(rotXRadians)*System.Math.Sin(rotYRadians)*System.Math.Cos(rotZRadians);
            m[0, 2] = System.Math.Sin(rotXRadians)*System.Math.Sin(rotZRadians) +
                      System.Math.Cos(rotXRadians)*System.Math.Sin(rotYRadians)*System.Math.Cos(rotZRadians);
            m[1, 0] = System.Math.Cos(rotYRadians)*System.Math.Sin(rotZRadians);
            m[1, 1] = System.Math.Cos(rotXRadians)*System.Math.Cos(rotZRadians) +
                      System.Math.Sin(rotXRadians)*System.Math.Sin(rotYRadians)*System.Math.Sin(rotZRadians);
            m[1, 2] = -System.Math.Sin(rotXRadians)*System.Math.Cos(rotZRadians) +
                      System.Math.Cos(rotXRadians)*System.Math.Sin(rotYRadians)*System.Math.Sin(rotZRadians);
            m[2, 0] = -System.Math.Sin(rotYRadians);
            m[2, 1] = System.Math.Sin(rotXRadians)*System.Math.Cos(rotYRadians);
            m[2, 2] = System.Math.Cos(rotXRadians)*System.Math.Cos(rotYRadians);
            return m;
        }

        /// <summary>
        ///     This creates a transformation matrix from left handed angle inputs of the rotation
        ///     on each axis (in degrees)
        /// </summary>
        /// <param name="rotXRadians">the left handed (clockwise) angle in degrees of rotation from the x axis</param>
        /// <param name="rotYRadians">the left handed (clockwise) angle in degrees of rotation from the y axis</param>
        /// <param name="rotZRadians">the left handed (clockwise) angle in degrees of rotation from the z axis</param>
        /// <returns>the rotation matrix for performing transforms</returns>
        public static Matrix3 GetRotationMatrix(double rotXDeg, double rotYDeg, double rotZDeg)
        {
            return GetRotationMatrixFromRadians(Conversions.DegreesToRadians(rotXDeg),
                Conversions.DegreesToRadians(rotYDeg), Conversions.DegreesToRadians(rotZDeg));
        }

        /// <summary>
        ///     Calculates the rotation and translation from a set of points in one coordinate system (orig) to another (transf).
        ///     You can use to create a new Matrix4 and transform points from (orig) to (trans) or take the inverse of the
        ///     resulting Matrix4
        ///     and transform (transf) back to (orig).
        /// </summary>
        /// <param name="orig">the points in the first coordinate system</param>
        /// <param name="transf">theh points in the second coordinate system</param>
        /// <returns>a tuple containing the rotation matrix as Item1 and the translation vector as Item2</returns>
        public static Matrix4 TransformBetween(IEnumerable<Vector3> orig, IEnumerable<Vector3> transf)
        {
            int rowCountA = orig.Count();
            int rowCountB = transf.Count();
            if (rowCountA != rowCountB)
            {
                throw new Exception("Data must be paired. The number of rows should be equal in both input matrices!");
            }
            Vector3 aCentroid = orig.GetCentroid();
            Vector3 bCentroid = transf.GetCentroid();

            double[,] aCentroidMatrix = aCentroid.RepMat(rowCountA);
            double[,] bCentroidMatrix = bCentroid.RepMat(rowCountB);

            double[,] a = orig.ToRowMatrix();
            double[,] b = transf.ToRowMatrix();

            double[,] h = (a.Subtract(aCentroidMatrix)).Transpose().Multiply(b.Subtract(bCentroidMatrix));

            SvdResult svd = h.Svd();
            double[,] U = svd.U;
            double[,] V = svd.Vt.Transpose();

            //TODO Consider reflection
            double[,] r = V.Multiply(U.Transpose());
            double[,] first = r.Multiply(-1);
            double[] second = first.Multiply(aCentroid);
            double[] t = r.Multiply(-1).Multiply(aCentroid).Add(bCentroid);
            return new Matrix4(new Matrix3(r), new Vector3(t));
        }
    }
}