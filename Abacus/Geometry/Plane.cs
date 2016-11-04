using System.Collections.Generic;

namespace Abacus.Geometry
{
    public class Plane
    {
        /// <summary>
        ///     Returns the bounded area on a plane between inside a parallelagram on the surface of the plane.
        /// </summary>
        /// <param name="c1">1st corner of the parallelagram</param>
        /// <param name="c2">2rd corner of the parallelagram</param>
        /// <param name="c3">3rd corner of the parallelagram</param>
        /// <returns>the bounded area</returns>
        public double BoundedArea(Vector3 c1, Vector3 c2, Vector3 c3)
        {
            return c1.DistanceTo(c2)*c2.DistanceTo(c3);
        }

        //public Vector3 GeneratePointOnPlane(Vector3 c1, Vector3 c2, Vector3 c3)
        //{
        //    Vector4 eq = this.AsPlaneEquation;
        //    double x, y, z;
        //    if (eq[2] != 0)
        //    {
        //        x = 100 * ThreadSafeRandom.Next();
        //        y = 100 * random.NextDouble();
        //        z = (eq[3] - eq[0] * x - eq[1] * y) / eq[2];
        //    }
        //    else if (eq[1] != 0)
        //    {
        //        x = 100 * random.NextDouble();
        //        z = 100 * random.NextDouble();
        //        y = (eq[3] - eq[0] * x - eq[2] * z) / eq[1];
        //    }
        //    else
        //    {
        //        y = 100 * random.NextDouble();
        //        z = 100 * random.NextDouble();
        //        x = (eq[3] - eq[1] * y - eq[2] * z) / eq[0];
        //    }
        //    return new Vector3(x, y, z);
        //}


        /// <summary>
        ///     Calculates the best fit plane through a list of 3D points
        /// </summary>
        /// <param name="points">the points which make up the plane</param>
        /// <returns>a vector [A,B,C] in the plane equation z = Ax + By + C</returns>
        public static Vector3 BestFit(IEnumerable<Vector3> points)
        {
            Matrix3 a = Matrix3.Zeroes;
            Vector3 b = Vector3.Zeroes;
            foreach (Vector3 point in points)
            {
                var mat = new Matrix3(new double[3, 3]
                {
                    {point.X*point.X, point.X*point.Y, point.X},
                    {point.X*point.Y, point.Y*point.Y, point.Y},
                    {point.X, point.Y, 1}
                });

                a = a + mat;
                b += new Vector3(point.X*point.Z, point.Y*point.Z, point.Z);
            }

            return new Vector3(a.Solve(b));
        }
    }
}