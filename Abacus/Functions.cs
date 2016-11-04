using System;
using System.Linq;
using M = System.Math;

namespace Abacus
{
    public class Functions
    {
        public static double Hypotenuse(double a, double b)
        {
            double r;
            if (M.Abs(a) > M.Abs(b))
            {
                r = b/a;
                r = M.Abs(a)*M.Sqrt(1 + r*r);
            }
            else if (b != 0)
            {
                r = a/b;
                r = M.Abs(b)*M.Sqrt(1 + r*r);
            }
            else
            {
                r = 0.0;
            }
            return r;
        }

        /// <summary>
        ///     Computes the error function for x. Adopted from http://www.johndcook.com/cpp_erf.html
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Erf(double x)
        {
            // constants
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

            // Save the sign of x
            int sign = 1;
            if (x < 0)
                sign = -1;
            x = M.Abs(x);

            // A&S formula 7.1.26
            double t = 1.0/(1.0 + p*x);
            double y = 1.0 - (((((a5*t + a4)*t) + a3)*t + a2)*t + a1)*t*M.Exp(-x*x);

            return sign*y;
        }

        /// <summary>
        ///     Performs linear interpolation between points (x1,y1) and (x3,y3) at the point xInterp. Moreover, it solves the
        ///     equation
        ///     $$ y_2 = \frac{(x_2-x_1)(y_3-y_1)}{x_3-x_1}+y1 $$
        /// </summary>
        /// <param name="x1">x1 in the equation</param>
        /// <param name="y1">y1 in the equation</param>
        /// <param name="x3">x3 in the equation</param>
        /// <param name="y3">y3 in the equation</param>
        /// <param name="x2">xInterp in the equation</param>
        /// <returns>y2 in the equation</returns>
        public static double Interpolate(double x1, double y1, double x3, double y3, double x2)
        {
            return (x2 - x1)*(y3 - y1)/(x3 - x1) + y1;
        }

        /// <summary>
        ///     Performs linear interpolation between points (x1,y1) and (x3,y3) at the point xInterp. Moreover, it solves the
        ///     equation
        ///     $$ y_2 = \frac{(x_2-x_1)(y_3-y_1)}{x_3-x_1}+y1 $$
        /// </summary>
        /// <param name="x1">x1 in the equation</param>
        /// <param name="y1">y1 in the equation</param>
        /// <param name="x3">x3 in the equation</param>
        /// <param name="y3">y3 in the equation</param>
        /// <param name="xInterp">xInterp in the equation</param>
        /// <returns>y2 in the equation</returns>
        public static double Interpolate(Vector2 point1, Vector2 point3, double xInterp)
        {
            return (xInterp - point1.X)*(point3.Y - point1.Y)/(point3.X - point1.X) + point1.Y;
        }

        /// <summary>
        ///     Performs bi-linear interpolation between points (x1,y1,value), (x1,y2,value), (x2,y2,value) at the point xInterp,
        ///     yInterp.
        ///     The z value of each point must be set to the value desired for interpolation.
        /// </summary>
        /// <param name="p1">the first known point</param>
        /// <param name="p2">the second known point</param>
        /// <param name="p3">the third known point</param>
        /// <param name="p4">the fourth known point</param>
        /// <param name="xInterp">the </param>
        /// <param name="yInterp"></param>
        /// <returns></returns>
        public static double BiInterpolate(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, double xInterp,
            double yInterp)
        {
            var points = new[] {p1, p2, p3, p4};
            double minX = points.Min((p => p.X));
            double minY = points.Min((p => p.Y));
            double maxX = points.Max((p => p.X));
            double maxY = points.Max((p => p.Y));

            if (points.Where(p => p.X == minX).Count() != 2 || points.Where(p => p.X == maxX).Count() != 2)
            {
                throw new ArgumentException("Points must be x1y1 x1y2 x2y1 x2y2");
            }
            if (points.Where(p => p.Y == minY).Count() != 2 || points.Where(p => p.Y == maxY).Count() != 2)
            {
                throw new ArgumentException("Points must be x1y1 x1y2 x2y1 x2y2");
            }

            double q11 = points.First(p => p.X == minX && p.Y == minY).Z;
            double q12 = points.First(p => p.X == minX && p.Y == maxY).Z;
            double q21 = points.First(p => p.X == maxX && p.Y == minY).Z;
            double q22 = points.First(p => p.X == maxX && p.Y == maxY).Z;

            double r1 = (maxX - xInterp)/(maxX - minX)*q11 + (xInterp - minX)/(maxX - minX)*q21;
            double r2 = (maxX - xInterp)/(maxX - minX)*q12 + (xInterp - minX)/(maxX - minX)*q22;
            return (maxY - yInterp)/(maxY - minY)*r1 + (yInterp - minY)/(maxY - minY)*r2;
        }

        internal static double Determinant(double[,] mat)
        {
            return Determinant(mat, mat.GetUpperBound(0) + 1);
        }

        internal static double Determinant(double[,] mat, int k)
        {
            int matM = mat.GetUpperBound(0) + 1;
            int matN = mat.GetUpperBound(1) + 1;

            if (matM != matN)
            {
                return double.NaN;
            } // Non-square

            double s = 1, det = 0;
            var b = new double[matM, matN];
            int i, j, m, n, c;
            if (k == 1)
            {
                return (mat[0, 0]);
            }
            det = 0;
            for (c = 0; c < k; c++)
            {
                m = 0;
                n = 0;
                for (i = 0; i < k; i++)
                {
                    for (j = 0; j < k; j++)
                    {
                        b[i, j] = 0;
                        if (i != 0 && j != c)
                        {
                            b[m, n] = mat[i, j];
                            if (n < (k - 2))
                                n++;
                            else
                            {
                                n = 0;
                                m++;
                            }
                        }
                    }
                }
                det = det + s*(mat[0, c]*Determinant(b, k - 1));
                s = -1*s;
            }
            return (det);
        }

        internal static double[,] Cofactors(double[,] mat)
        {
            int f = mat.GetUpperBound(0) + 1;
            var b = new double[f, f];
            var factors = new double[f, f];
            int p, q, m, n, i, j;
            for (q = 0; q < f; q++)
            {
                for (p = 0; p < f; p++)
                {
                    m = 0;
                    n = 0;
                    for (i = 0; i < f; i++)
                    {
                        for (j = 0; j < f; j++)
                        {
                            b[i, j] = 0;
                            if (i != q && j != p)
                            {
                                b[m, n] = mat[i, j];
                                if (n < (f - 2))
                                    n++;
                                else
                                {
                                    n = 0;
                                    m++;
                                }
                            }
                        }
                    }
                    factors[q, p] = M.Pow(-1, q + p)*Determinant(b, f - 1);
                }
            }
            return factors;
        }
    }
}