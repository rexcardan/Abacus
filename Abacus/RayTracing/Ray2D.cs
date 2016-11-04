namespace Abacus.RayTracing
{
    public class Ray2D
    {
        public Ray2D(Vector2 source, Vector2 direction)
        {
            Source = source;
            Direction = direction;
        }

        public Vector2 Source { get; set; }
        public Vector2 Direction { get; set; }

        /// <summary>
        ///     Finds the intersection if one exists between the current ray and the input ray
        /// </summary>
        /// <param name="ray">the ray to intersect</param>
        /// <returns>the 2D coordinates of the intersection</returns>
        public Vector2 Intersect(Ray2D ray)
        {
            double x1 = Source.X;
            double y1 = Source.Y;
            double x2 = Direction.X;
            double y2 = Direction.Y;

            double x3 = ray.Source.X;
            double y3 = ray.Source.Y;
            double x4 = ray.Direction.X;
            double y4 = ray.Direction.Y;

            double denom = (x1 - x2)*(y3 - y4) - (y1 - y2)*(x3 - x4);
            double xNum = ((x1*y2 - y1*x2)*(x3 - x4) - (x1 - x2)*(x3*y4 - y3*x4));
            double yNum = ((x1*y2 - y1*x2)*(y3 - y4) - (y1 - y2)*(x3*y4 - y3*x4));
            if (System.Math.Abs(denom - 0) < 0.00000001) denom = 0; //Practically zero                 
            return new Vector2(xNum/denom, yNum/denom);
        }
    }
}