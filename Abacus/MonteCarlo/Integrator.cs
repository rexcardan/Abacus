using System.Threading;
using System.Threading.Tasks;
using Abacus.Geometry;

namespace Abacus.MonteCarlo
{
    public class Integrator
    {
        /// <summary>
        ///     Calculates the area overlap between two shapes by throwing random darts within the bounds of the
        ///     first shape and counting the darts that lie within both shapes vs total darts thrown.
        /// </summary>
        /// <param name="shape1">the first shape</param>
        /// <param name="shape2">the second shape</param>
        /// <param name="numOfSamples">the number of darts to throw. The more darts the more accurate the simulation.</param>
        /// <returns></returns>
        public static double FindOverlappingArea(IShape2D shape1, IShape2D shape2, long numOfSamples)
        {
            long overlap = 0;

            Parallel.For(0, numOfSamples, i =>
            {
                Vector2 dart = GenerateDartInShapeBounds(shape1);
                if (shape1.ContainsPoint(dart) && shape2.ContainsPoint(dart))
                {
                    Interlocked.Increment(ref overlap);
                }
            });

            double boundedArea = (shape1.MaxX - shape1.MinX)*(shape1.MaxY - shape1.MinY);
            return (double) overlap/numOfSamples*boundedArea;
        }

        private static Vector2 GenerateDartInShapeBounds(IShape2D shape)
        {
            double randW = ThreadSafeRandom.Next()*(shape.MaxX - shape.MinX) + shape.MinX;
            double randH = ThreadSafeRandom.Next()*(shape.MaxY - shape.MinY) + shape.MinY;
            var dart = new Vector2(randW, randH);
            return dart;
        }
    }
}