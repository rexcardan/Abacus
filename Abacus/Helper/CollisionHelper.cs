using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.Geometry;
using Abacus.RayTracing;

namespace Abacus.Helper
{
    public static class CollisionHelper
    {
        public static bool FindIntersection(Ray3D ray, Triangle3D t, out bool isOnEdge)
        {
            double u, v, distance = 0;
            Vector3 d = ray.Destination;
            Vector3 T = ray.Source - t.P1;
            if (T == null) throw new ArgumentNullException("T");

            Vector3 edge1 = t.P2 - t.P1;
            Vector3 edge2 = t.P3 - t.P1;

            Vector3 P = d.CrossMultiply(edge2);
            Vector3 Q = T.CrossMultiply(edge1);

            float row1, row2, row3;
            row1 = (float) (Q*edge2);
            row2 = (float) (P*T);
            row3 = (float) (Q*d);

            Vector3 result = new Vector3(row1, row2, row3)*(1.0f/(P*edge1));
            distance = result[0];
            u = result[1];
            v = result[2];

            bool test = u == 0;
            bool test2 = v == 0;

            isOnEdge = test || test2;

            return (u >= 0 &&
                    v >= 0 &&
                    distance >= 0 &&
                    u + v <= 1);
        }

        public static bool IsInsideMesh(Vector3 point, IList<Triangle3D> mesh)
        {
            var xAxis = new Vector3(1, 0, 0);
            var yAxis = new Vector3(0, 1, 0);
            var zAxis = new Vector3(0, 0, 1);

            return AxisTest(point, mesh, xAxis) && AxisTest(point, mesh, yAxis) && AxisTest(point, mesh, zAxis);
        }

        private static bool AxisTest(Vector3 point, IList<Triangle3D> mesh, Vector3 axis)
        {
            var r = new Ray3D(new Vector3(point.X, point.Y, point.Z), axis);
            double totalCount = 0;
            foreach (Triangle3D t in mesh)
            {
                bool isOnEdge;
                bool isColl = FindIntersection(r, t, out isOnEdge);
                if (isColl)
                {
                    if (isOnEdge)
                    {
                        totalCount += 0.5;
                    }
                    else
                    {
                        totalCount++;
                    }
                }
            }
            return totalCount%2 != 0; //Is Odd
        }

        public static List<Triangle3D> GetBoundingBox(IList<Vector3> points)
        {
            var boxTris = new List<Triangle3D>();

            double minX = points.Min(p => p.X);
            double maxX = points.Max(p => p.X);
            double minY = points.Min(p => p.Y);
            double maxY = points.Max(p => p.Y);
            double minZ = points.Min(p => p.Z);
            double maxZ = points.Max(p => p.Z);

            //MAX X PLANE
            var p1 = new Vector3(maxX, minY, minZ);
            var p2 = new Vector3(maxX, maxY, minZ);
            var p3 = new Vector3(maxX, maxY, maxZ);
            var p4 = new Vector3(maxX, minY, maxZ);

            //MAX Y PLANE (Not Already calculated)
            var p5 = new Vector3(minX, maxY, minZ);
            var p6 = new Vector3(minX, maxY, maxZ);

            //MIN Y PLANE (Not Already calculated)
            var p7 = new Vector3(minX, minY, minZ);
            var p8 = new Vector3(minX, minY, maxZ);

            //Min Y Plane
            boxTris.Add(new Triangle3D(p4, p8, p1));
            boxTris.Add(new Triangle3D(p8, p7, p1));
            //Max Y Plane
            boxTris.Add(new Triangle3D(p3, p5, p6));
            boxTris.Add(new Triangle3D(p3, p2, p5));
            //Min X Plane
            boxTris.Add(new Triangle3D(p6, p5, p8));
            boxTris.Add(new Triangle3D(p5, p7, p8));
            //Max X Plane
            boxTris.Add(new Triangle3D(p4, p2, p3));
            boxTris.Add(new Triangle3D(p4, p1, p2));
            //Min Z Plane
            boxTris.Add(new Triangle3D(p5, p2, p7));
            boxTris.Add(new Triangle3D(p7, p2, p1));
            //Max Z Plane
            boxTris.Add(new Triangle3D(p3, p6, p8));
            boxTris.Add(new Triangle3D(p8, p4, p3));

            return boxTris;
        }

        public static List<Triangle3D> GetBoundingBox(IList<Triangle3D> triangles)
        {
            var points = new List<Vector3>();
            foreach (Triangle3D tri in triangles)
            {
                points.Add(tri.P1);
                points.Add(tri.P2);
                points.Add(tri.P3);
            }
            return GetBoundingBox(points);
        }
    }
}