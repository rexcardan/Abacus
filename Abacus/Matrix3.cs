using Abacus.Helper;
using Abacus.Interface;

namespace Abacus
{
    public class Matrix3 : SquareMatrix<Matrix3>, IMatrix
    {
        #region CONSTRUCTORS

        public Matrix3()
            : base(MatrixHelper.Fill(3, 0))
        {
        }

        /// <summary>
        ///     Double array constructor. Initializes matrix to contain the values of a double array. The double
        ///     array should be double[3,3] for Matrix3 methods to work properly
        /// </summary>
        /// <param name="values"></param>
        public Matrix3(double[,] values) : base(values, 3)
        {
        }

        /// <summary>
        ///     Vector constructor for 3 x 3 matrix. Initializes rows as input vectors.
        /// </summary>
        /// <param name="row1">Vector3 representing the first row of the matrix</param>
        /// <param name="row2">Vector3 representing the second row of the matrix</param>
        /// <param name="row3">Vector3 representing the third row of the matrix</param>
        public Matrix3(Vector3 row1, Vector3 row2, Vector3 row3)
            : base(new double[3, 3]
            {
                {row1[0], row1[1], row1[2]},
                {row2[0], row2[1], row2[2]},
                {row3[0], row3[1], row3[2]}
            })
        {
        }

        #endregion

        #region METHODS

        public static Matrix3 Zeroes
        {
            get { return MatrixHelper.Fill<Matrix3>(3, 0); }
        }

        public static Matrix3 Identity
        {
            get { return new Matrix3(MatrixHelper.Identity(3)); }
        }

        #endregion

        #region OPERATORS

        /// <summary>
        ///     The Matrix vector product operator.
        /// </summary>
        /// <param name="m1">matrix to be multiplied</param>
        /// <param name="v1">vector to be multiplied</param>
        /// <returns>a new vector containing the multiplied values</returns>
        public static Vector3 operator *(Matrix3 m1, Vector3 v1)
        {
            return new Vector3(
                (m1[0, 0]*v1[0] + m1[0, 1]*v1[1] + m1[0, 2]*v1[2]),
                (m1[1, 0]*v1[0] + m1[1, 1]*v1[1] + m1[1, 2]*v1[2]),
                (m1[2, 0]*v1[0] + m1[2, 1]*v1[1] + m1[2, 2]*v1[2])
                );
        }

        /// <summary>
        ///     The Matrix vector product operator.
        /// </summary>
        /// <param name="m1">matrix to be multiplied</param>
        /// <param name="v1">vector to be multiplied</param>
        /// <returns>a new vector containing the multiplied values</returns>
        public static Vector3 operator *(Vector3 v1, Matrix3 m1)
        {
            return new Vector3(
                (m1[0, 0]*v1[0] + m1[0, 1]*v1[1] + m1[0, 2]*v1[2]),
                (m1[1, 0]*v1[0] + m1[1, 1]*v1[1] + m1[1, 2]*v1[2]),
                (m1[2, 0]*v1[0] + m1[2, 1]*v1[1] + m1[2, 2]*v1[2])
                );
        }

        #endregion
    }
}