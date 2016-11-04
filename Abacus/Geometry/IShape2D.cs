namespace Abacus.Geometry
{
    public interface IShape2D
    {
        double MinX { get; }
        double MaxX { get; }
        double MinY { get; }
        double MaxY { get; }
        double Area { get; }
        bool ContainsPoint(Vector2 v);
    }
}