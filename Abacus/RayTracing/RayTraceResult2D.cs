namespace Abacus.RayTracing
{
    public class RayTraceResult2D
    {
        public Vector2 EntryPoint { get; set; }
        public Vector2 ExitPoint { get; set; }
        public double PriorLength { get; set; }

        public double InnerLength
        {
            get { return (EntryPoint - ExitPoint).Norm(); }
        }
    }
}