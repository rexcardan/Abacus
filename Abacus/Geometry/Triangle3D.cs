using System;

namespace Abacus.Geometry
{
    public class Triangle3D
    {
        /// <summary>
        ///     A constructor for creating a triangle
        /// </summary>
        /// <param name="p1">A vector representing the X, Y, and Z coordinates of the first vertex in the triangle</param>
        /// <param name="p2">A vector representing the X, Y, and Z coordinates of the second vertex in the triangle</param>
        /// <param name="p3">A vector representing the X, Y, and Z coordinates of the third vertex in the triangle</param>
        public Triangle3D(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }

        /// <summary>
        ///     A constructor for creating a triangle
        /// </summary>
        /// <param name="p1">A vector representing the X, Y, and Z coordinates of the first vertex in the triangle</param>
        /// <param name="p2">A vector representing the X, Y, and Z coordinates of the second vertex in the triangle</param>
        /// <param name="p3">A vector representing the X, Y, and Z coordinates of the third vertex in the triangle</param>
        /// <param name="id">The id of the triangle (must be unique)</param>
        public Triangle3D(Vector3 p1, Vector3 p2, Vector3 p3, int id = 0)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            Id = id;
        }


        public Triangle3D(double[] p1, double[] p2, double[] p3, int id = 0)
        {
            P1 = new Vector3(p1);
            P2 = new Vector3(p2);
            P3 = new Vector3(p3);
            Id = id;
        }

        public Vector3 P1 { get; set; }
        public Vector3 P2 { get; set; }
        public Vector3 P3 { get; set; }
        public int Id { get; set; }

        public double MinX
        {
            get { throw new NotImplementedException(); }
        }

        public double MaxX
        {
            get { throw new NotImplementedException(); }
        }

        public double MinY
        {
            get { throw new NotImplementedException(); }
        }

        public double MaxY
        {
            get { throw new NotImplementedException(); }
        }

        public double Area
        {
            get { throw new NotImplementedException(); }
        }

        public Vector3 GetCenter()
        {
            double x = (P1.X + P2.X + P3.X)/3;
            double y = (P1.Y + P2.Y + P3.Y)/3;
            double z = (P1.Z + P2.Z + P3.Z)/3;
            return new Vector3(x, y, z);
        }

        public bool ContainsPoint(Vector2 v)
        {
            throw new NotImplementedException();
        }
    }
}