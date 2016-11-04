namespace Abacus.Interface
{
    public interface IMatrix
    {
        double[,] Values { get; }
        double this[long row, long column] { get; set; }
        int M { get; }
        int N { get; }
    }
}