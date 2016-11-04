namespace Abacus.Geometry
{
    public struct Square2D : IShape2D
    {
        private Vector2 _lowXyCorner;
        private double _sideLength;

        public Square2D(Vector2 lowXyCorner, double sideLength)
        {
            _lowXyCorner = lowXyCorner;
            _sideLength = sideLength;
        }

        public double SideLength
        {
            get { return _sideLength; }
            set { _sideLength = value; }
        }

        public Vector2 LowXyCorner
        {
            get { return _lowXyCorner; }
            set { _lowXyCorner = value; }
        }

        public double MinX
        {
            get { return LowXyCorner.X; }
        }

        public double MaxX
        {
            get { return LowXyCorner.X + SideLength; }
        }

        public double MinY
        {
            get { return LowXyCorner.Y; }
        }

        public double MaxY
        {
            get { return LowXyCorner.Y + SideLength; }
        }

        public bool ContainsPoint(Vector2 v)
        {
            return (v.X >= MinX && v.X <= MaxX) && (v.Y >= MinY && v.Y <= MaxY);
        }

        public double Area
        {
            get { return SideLength*SideLength; }
        }

        public Vector2[] GetCorners()
        {
            double x = LowXyCorner.X;
            double y = LowXyCorner.Y;

            return new[]
            {
                new Vector2(MinX, MinY),
                new Vector2(MaxX, MaxY),
                new Vector2(MaxX, MinY),
                new Vector2(MinX, MaxY)
            };
        }
    }
}