using Abacus.Geometry;

namespace Abacus.Helper
{
    public class AngleHelper
    {
        /// <summary>
        ///     Returns true if test angle is between startAngle and endAngle, else returns false
        /// </summary>
        /// <param name="testAngle">the angle to test</param>
        /// <param name="startAngle">the start angle marking the beginning bounds</param>
        /// <param name="endAngle">the end angle marking the end bounds</param>
        /// <returns>whether the test angle is inside the range</returns>
        public static bool IsInsideRange(double testAngle, double startAngle, double endAngle)
        {
            double a1 = System.Math.Abs(AngleBetween(startAngle, testAngle));
            double a2 = System.Math.Abs(AngleBetween(testAngle, endAngle));
            double a3 = System.Math.Abs(AngleBetween(startAngle, endAngle));
            return a1 + a2 == a3;
        }

        public static double AngleBetween(double start, double end)
        {
            return (end - start)%360;
        }

        public static double Normalize(double angle)
        {
            while (angle <= -180) angle += 360;
            while (angle > 180) angle -= 360;
            return angle;
        }

        public static double Cos(double angle, DegreeType degType = DegreeType.Radians)
        {
            angle = degType == DegreeType.Degrees ? (System.Math.PI/180)*angle : angle;
            return System.Math.Cos(angle);
        }

        public static double Tan(double angle, DegreeType degType = DegreeType.Radians)
        {
            angle = degType == DegreeType.Degrees ? (System.Math.PI/180)*angle : angle;
            return System.Math.Tan(angle);
        }

        public static double Sin(double angle, DegreeType degType = DegreeType.Radians)
        {
            angle = degType == DegreeType.Degrees ? (System.Math.PI/180)*angle : angle;
            return System.Math.Sin(angle);
        }

        public static double ToRadian(double angle, DegreeType degType)
        {
            return degType == DegreeType.Degrees ? (System.Math.PI/180)*angle : angle;
        }
    }
}