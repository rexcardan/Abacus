namespace Abacus.Geometry
{
    public struct Circle2D : IShape2D
    {
        private Vector2 _center;

        private double _radius;

        public Circle2D(Vector2 center, double radius)
        {
            _center = center;
            _radius = radius;
        }

        public Vector2 Center
        {
            get { return _center; }
            set { _center = value; }
        }

        public double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        public double MinX
        {
            get { return Center.X - Radius; }
        }

        public double MaxX
        {
            get { return Center.X + Radius; }
        }

        public double MinY
        {
            get { return Center.Y - Radius; }
        }

        public double MaxY
        {
            get { return Center.Y + Radius; }
        }

        public bool ContainsPoint(Vector2 v)
        {
            float dist = v.DistanceTo(Center);
            return dist <= Radius;
        }

        public double Area
        {
            get { return System.Math.PI*Radius*Radius; }
        }
    }
}