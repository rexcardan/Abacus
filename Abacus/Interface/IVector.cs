namespace Abacus.Interface
{
    public interface IVector
    {
        double[] Values { get; }
        double this[long el] { get; set; }
        int Length { get; }
    }
}