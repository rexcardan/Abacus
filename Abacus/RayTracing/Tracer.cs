using System.Collections.Generic;
using System.Linq;

namespace Abacus.RayTracing
{
    public class Tracer
    {
        private static double Epsi = 0.0000001;

        /// <summary>
        ///     Fires a ray through a pixel grid and calculates a ray trace result for each pixel crossed.
        /// </summary>
        /// <param name="ray">the ray to fire</param>
        /// <param name="pixelGrid">the pixel grid with which to calculate the results should be dimensions [x,y]</param>
        /// <param name="xWidth">the width of each pixel in the x dimension</param>
        /// <param name="yWidth">the height of each pixel in the y direction</param>
        /// <param name="origin">the origin of the grid</param>
        /// <returns>a matrix of ray trace results for each pixel in the grid, null if no interaction</returns>
        public static RayTraceResult2D[,] Fire(Ray2D ray, byte[,] pixelGrid, double xWidth = 1.0, double yWidth = 1.0,
            Vector2 origin = null)
        {
            origin = origin ?? Vector2.Zeroes;
            int rowCount = pixelGrid.GetLength(1);
            int columnCount = pixelGrid.GetLength(0);

            var hzLines = Enumerable.Range(0, rowCount + 1)
                .Select((h, i) => new {Index = i, Line = origin.Y + yWidth*h}).ToList();
            var vLines = Enumerable.Range(0, columnCount + 1)
                .Select((v, i) => new {Index = i, Line = origin.X + xWidth*v}).ToList();

            int maxXIndex = columnCount;
            int maxYIndex = rowCount;

            var hIntersects =
                hzLines.Select(
                    y => new {Intersect = new Ray2D(new Vector2(0, y.Line), new Vector2(1, y.Line)).Intersect(ray)})
                    .ToList();
            var vIntersects =
                vLines.Select(
                    x => new {Intersect = new Ray2D(new Vector2(x.Line, 0), new Vector2(x.Line, 1)).Intersect(ray)})
                    .ToList();

            var allIntersects =
                vIntersects.Concat(hIntersects).OrderBy(i => i.Intersect.DistanceTo(ray.Source)).ToList();

            //BIN ALL INTERSECTIONS INTO PIXELS
            var binned = new List<Vector2>[columnCount, rowCount];
            for (int i = 0; i < binned.GetLength(0); i++)
            {
                for (int j = 0; j < binned.GetLength(1); j++)
                {
                    binned[i, j] = new List<Vector2>();
                }
            }

            for (int i = 0; i < allIntersects.Count - 1; i++)
            {
                double x = allIntersects[i].Intersect.X;
                double y = allIntersects[i].Intersect.Y;

                if ((x >= origin.X && x < origin.X + maxXIndex*xWidth) &&
                    (y >= origin.Y && y < origin.Y + maxYIndex*yWidth))
                {
                    int row = hzLines.Count(h => h.Line <= allIntersects[i].Intersect.Y - Epsi);
                    int column = vLines.Count(v => v.Line <= allIntersects[i].Intersect.X - Epsi);
                    binned[column, row].Add(allIntersects[i].Intersect);
                    binned[column, row].Add(allIntersects[i + 1].Intersect);
                }
            }

            //ORGANIZE RESULTS
            var results = new RayTraceResult2D[columnCount, rowCount];
            for (int x = 0; x < columnCount; x++)
            {
                for (int y = 0; y < rowCount; y++)
                {
                    if (binned[x, y].Any())
                    {
                        Vector2[] inOrder = binned[x, y].Distinct().OrderBy(i => i.DistanceTo(ray.Source)).ToArray();
                        Vector2 entryRay = inOrder[0] - ray.Source;
                        double test1 = entryRay*ray.Source;
                        double test2 = entryRay*ray.Direction;

                        if (test1 >= 0 && test2 >= 0)
                        {
                            results[x, y] = new RayTraceResult2D
                            {
                                EntryPoint = inOrder[0],
                                ExitPoint = inOrder[1],
                                PriorLength = (inOrder[0] - ray.Source).Norm()
                            };
                            pixelGrid[x, y] = 255;
                        }
                    }
                }
            }
            return results;
        }
    }
}