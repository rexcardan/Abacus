using System;

namespace Abacus
{
    public static class MatrixExtensions
    {
        public static Matrix3 ToMatrix3(this double[,] array)
        {
            if (array.GetUpperBound(0) == 2 && array.GetUpperBound(1) == 2)
            {
                return new Matrix3(array);
            }
            throw new Exception("Cannot construct matrix 3 from this array. Wrong size.");
        }

        public static Matrix4 ToMatrix4(this double[,] array)
        {
            if (array.GetUpperBound(0) == 3 && array.GetUpperBound(1) == 3)
            {
                return new Matrix4(array);
            }
            throw new Exception("Cannot construct matrix 4 from this array. Wrong size.");
        }

        public static Matrix4 ToMatrix4(this Tuple<Matrix3, Vector3> rotTrans)
        {
            return new Matrix4(rotTrans.Item1, rotTrans.Item2);
        }
    }
}