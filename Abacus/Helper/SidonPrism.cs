using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.RayTracing;

namespace Abacus.Helper
{
    public class SidonPrism
    {
        public SidonPrism(double sliceThickness)
        {
            YSliceThickness = sliceThickness;
        }

        /// <summary>
        ///     The thickness to extrude the polygon for intersection test. Needs to be at least a high as the Y distance to the
        ///     next polygon.
        /// </summary>
        public double YSliceThickness { get; set; }

        public bool DoesIntersect(Ray3D ray, List<Vector3> polyPoints, out double entry, out double depth)
        {
            double aMin;
            double aMax;


            GetAminAmax(ray, polyPoints, out aMin, out aMax);
            List<double> cKs = GetCks(ray, polyPoints);

            List<double> intersections = FindIntersectionParameters(ray, cKs, polyPoints);

            float rayLength = ray.Source.DistanceTo(ray.Destination);
            depth = GetRayDepth(rayLength, intersections, aMin, aMax);
            entry = 0;
            return true;
        }

        private double GetRayDepth(double rayLength, List<double> intersections, double aMin, double aMax)
        {
            double depth = 0;
            for (int i = 0; i < intersections.Count; i++)
            {
                int multiplier = i%2 == 0 ? 1 : -1; // odd = negative, even = positive
                depth += (new List<double> {new List<double> {intersections[i], aMin}.Max(), aMax}.Min())*multiplier;
            }
            return rayLength*depth;
        }

        private List<double> FindIntersectionParameters(Ray3D ray, List<double> cKs, List<Vector3> polyPoints)
        {
            //FLAGS
            bool fPlus0 = false;
            bool fMinus0 = false;
            Vector3 pPlus0 = Vector3.NaN;
            Vector3 pMinus0 = Vector3.NaN;

            var intersections = new List<double>();

            for (int i = 0; i < cKs.Count - 1; i++)
            {
                if (cKs[i] > 0) // CK +
                {
                    if (cKs[i + 1] == 0)
                    {
                        fPlus0 = true;
                        pPlus0 = polyPoints[i + 1];
                        fMinus0 = false;
                    }
                    if (cKs[i + 1] < 0)
                    {
                        double alpha = GetIntersectionParameter(ray.Source, polyPoints[i], polyPoints[i + 1], cKs[i],
                            cKs[i + 1]);
                        intersections.Add(alpha);
                    }
                }
                else if (cKs[i] == 0) // CK = 0
                {
                    if (cKs[i + 1] > 0)
                    {
                        if (fMinus0)
                        {
                            fMinus0 = false;
                            Vector3 midPoint = GetMidPoint(polyPoints[i], pMinus0);
                            double alpha = GetIntersectionParameter(ray.Source, midPoint, polyPoints[i + 1], cKs[i],
                                cKs[i + 1]);
                            intersections.Add(alpha);
                            fPlus0 = false;
                        }
                    }
                    else if (cKs[i + 1] < 0)
                    {
                        if (fPlus0)
                        {
                            fPlus0 = false;
                            Vector3 midPoint = GetMidPoint(polyPoints[i], pPlus0);
                            double alpha = GetIntersectionParameter(ray.Source, midPoint, polyPoints[i + 1], cKs[i],
                                cKs[i + 1]);
                            intersections.Add(alpha);
                            fMinus0 = false;
                        }
                    }
                }
                else if (cKs[i] < 0) // CK -
                {
                    if (cKs[i + 1] > 0)
                    {
                        double alpha = GetIntersectionParameter(ray.Source, polyPoints[i], polyPoints[i + 1], cKs[i],
                            cKs[i + 1]);
                        intersections.Add(alpha);
                    }
                    else if (cKs[i + 1] == 0)
                    {
                        fMinus0 = true;
                        pMinus0 = polyPoints[i + 1];
                        fPlus0 = false;
                    }
                }
            }

            intersections.OrderBy(d => d); //Sort highest first

            return intersections;
        }

        private Vector3 GetMidPoint(Vector3 point1, Vector3 point2)
        {
            return new Vector3((point1.X + point2.X)/2, (point1.Y + point2.Y)/2, (point1.Z + point2.Z)/2);
        }

        private double GetIntersectionParameter(Vector3 a, Vector3 k, Vector3 kPlus1, double ck, double cKplus1)
        {
            return ((k.X - a.X)/(kPlus1.Z - a.Z) - (k.Z - a.Z)/(kPlus1.X - a.X))/(ck - cKplus1);
        }

        private List<double> GetCks(Ray3D ray, List<Vector3> polyPoints)
        {
            double xa = ray.Source.X;
            double xb = ray.Destination.X;
            double za = ray.Source.Z;
            double zb = ray.Destination.Z;

            int i = 0;

            while ((CalcCk(polyPoints[i], xa, za, xb, zb)) == 0 && (i < polyPoints.Count))
            {
                i++;
            }
            if (i == polyPoints.Count)
            {
                throw new Exception("Polygon encloses no area. Cannot calculate intersection.");
            }
            //Reorder points so that the first ck is not zero 
            for (int j = 0; j < i; j++)
            {
                Vector3 movingPoint = polyPoints[j];
                polyPoints.Remove(movingPoint);
                polyPoints.Add(movingPoint);
            }

            return polyPoints.Select(p => CalcCk(p, xa, za, xb, zb)).ToList();
        }

        private double CalcCk(Vector3 point, double xa, double za, double xb, double zb)
        {
            double xk = point.X;
            double zk = point.Z;

            return (xk - xa)*(zb - za) - (zk - za)*(xb - xa);
        }

        /// <summary>
        ///     Returns the amin and amax values from the Sidon Prism paper for a given Y slice thickness.
        /// </summary>
        /// <param name="ray">the incoming ray</param>
        /// <param name="polyPoints">the plane of polygon points (X,Z). Should be all in the same Y coordinate</param>
        /// <param name="aMin">the a_min parameter from Sidon Prism paper</param>
        /// <param name="aMax">the a_max parameter from Sidon Prism paper.</param>
        private void GetAminAmax(Ray3D ray, List<Vector3> polyPoints, out double aMin, out double aMax)
        {
            double ya = ray.Source.Y;
            double yb = ray.Destination.Y;
            double y1 = polyPoints[0].Y; //Assumes all points have same y value
            double y2 = y1 + YSliceThickness; // Positive Y extrusion

            if (ya < yb) // Eq. 6a
            {
                aMin = new List<double> {0, (y1 - ya)/(yb - ya)}.Max();
                aMax = new List<double> {1, (y2 - ya)/(yb - ya)}.Min();
            }
            else if (ya > yb) // Eq. 6b
            {
                aMin = new List<double> {0, (y2 - ya)/(yb - ya)}.Max();
                aMax = new List<double> {1, (y1 - ya)/(yb - ya)}.Min();
            }
            else // Equal Ys
            {
                aMin = 0;
                aMax = 1;
            }
        }
    }
}