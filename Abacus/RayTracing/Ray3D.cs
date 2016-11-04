namespace Abacus.RayTracing
{
    public class Ray3D
    {
        public Ray3D(Vector3 source, Vector3 destination)
        {
            Source = source;
            Destination = destination;
        }

        public Vector3 Source { get; set; }
        public Vector3 Destination { get; set; }
    }
}