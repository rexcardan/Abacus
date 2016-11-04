using System;
using Abacus.Exceptions;
using Abacus.Helper;
using Abacus.Interface;

namespace Abacus
{
    public class SquareMatrix<T> where T : IMatrix
    {
        #region CONSTRUCTORS

        protected SquareMatrix(double[,] values)
        {
            if (values.GetUpperBound(0) != values.GetUpperBound(1))
            {
                throw new Exception(string.Format("Array dimensions must be a square."));
            }
            Values = values;
        }

        /// <summary>
        ///     Double array constructor. Initializes matrix to contain the values of a double array.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="dim">the dimensions of the square (eg. 3 for a 3x3)</param>
        public SquareMatrix(double[,] values, int dim)
        {
            if (values.GetUpperBound(0) != dim - 1)
            {
                throw new InvalidSizeException(dim, values.GetUpperBound(0));
            }
            if (values.GetUpperBound(1) != dim - 1)
            {
                throw new InvalidSizeException(dim, values.GetUpperBound(1));
            }
            Values = values;
        }

        #endregion

        #region ACCESSORS

        /// <summary>
        ///     Accessor for matrix so that an element can be obtained by m[row, column]
        /// </summary>
        /// <param name="row">the row containing the element</param>
        /// <param name="column">the column containing the element</param>
        /// <returns>a double value that is the element of the selected row and column</returns>
        public double this[long row, long column]
        {
            get
            {
                if (row > Values.GetUpperBound(0))
                {
                    string msg = string.Format("This matrix only has {0} rows", Values.GetUpperBound(0));
                    throw new IndexOutOfRangeException(msg);
                }

                if (column > Values.GetUpperBound(1))
                {
                    string msg = string.Format("This matrix only has {0} columns", Values.GetUpperBound(1));
                    throw new IndexOutOfRangeException(msg);
                }
                return Values[row, column];
            }
            set
            {
                if (row > Values.GetUpperBound(0))
                {
                    string msg = string.Format("This matrix only has {0} rows", Values.GetUpperBound(0));
                    throw new IndexOutOfRangeException(msg);
                }

                if (column > Values.GetUpperBound(1))
                {
                    string msg = string.Format("This matrix only has {0} columns", Values.GetUpperBound(1));
                    throw new IndexOutOfRangeException(msg);
                }
                Values[row, column] = value;
            }
        }

        #endregion

        #region PROPERTIES

        public double[,] Values { get; set; }

        /// <summary>
        ///     The number of rows
        /// </summary>
        public int M
        {
            get { return Values.RowCount(); }
        }

        /// <summary>
        ///     The number of columns
        /// </summary>
        public int N
        {
            get { return Values.ColumnCount(); }
        }

        /// <summary>
        ///     Returns the determinant for this matrix.
        /// </summary>
        public double Determinant
        {
            get { return Values.Determinant(); }
        }

        public T Solid
        {
            get { return (T) Activator.CreateInstance(typeof (T), Values); }
        }

        #endregion

        #region OPERATORS

        /// <summary>
        ///     Adds two matrices
        /// </summary>
        /// <param name="m1">the first matrix</param>
        /// <param name="m2">the second matrix</param>
        /// <returns>the matrix which is the sum of the two inputs</returns>
        public static T operator +(SquareMatrix<T> m1, SquareMatrix<T> m2)
        {
            if (m1.M != m2.M)
            {
                throw new Exception("Matrices are not same size! Cannot add.");
            }

            double[,] result = m1.Values;

            for (int m = 0; m < m1.M; m++)
            {
                for (int n = 0; n < m1.N; n++)
                {
                    result[m, n] += m2[m, n];
                }
            }
            return (T) Activator.CreateInstance(typeof (T), result);
        }

        /// <summary>
        ///     Subtracts two matrices
        /// </summary>
        /// <param name="m1">the first matrix</param>
        /// <param name="m2">the second matrix</param>
        /// <returns>the matrix which is the subtraction of the two inputs</returns>
        public static T operator -(SquareMatrix<T> m1, SquareMatrix<T> m2)
        {
            if (m1.M != m2.M)
            {
                throw new Exception("Matrices are not same size! Cannot subtract.");
            }

            double[,] result = m1.Values;

            for (int m = 0; m < m1.M; m++)
            {
                for (int n = 0; n < m1.M; n++)
                {
                    result[m, n] -= m2[m, n];
                }
            }
            return (T) Activator.CreateInstance(typeof (T), result);
        }

        /// <summary>
        ///     Scalar multiplication operator for the Matrix
        /// </summary>
        /// <param name="d">the scalar to multiply each element by</param>
        /// <param name="m2">the matrix of which the elements will be multiplied</param>
        /// <returns>a new matrix3 containing the multiplied values</returns>
        public static T operator *(double scalar, SquareMatrix<T> m1)
        {
            double[,] result = m1.Values;

            for (int m = 0; m < m1.M; m++)
            {
                for (int n = 0; n < m1.M; n++)
                {
                    result[m, n] *= scalar;
                }
            }

            return (T) Activator.CreateInstance(typeof (T), result);
        }

        /// <summary>
        ///     Scalar multiplication operator for the Matrix
        /// </summary>
        /// <param name="d">the scalar to multiply each element by</param>
        /// <param name="m2">the matrix of which the elements will be multiplied</param>
        /// <returns>a new matrix3 containing the multiplied values</returns>
        public static T operator *(SquareMatrix<T> m1, double scalar)
        {
            return scalar*m1;
        }

        /// <summary>
        ///     Traditional matrix multiplication operator between to matrices A and B
        /// </summary>
        /// <param name="m1">the first matrix A to get the matrix AB </param>
        /// <param name="m2">the second matrix B to get the matrix AB </param>
        /// <returns>the matrix AB from matrices A and B</returns>
        public static T operator *(SquareMatrix<T> m1, SquareMatrix<T> m2)
        {
            double[,] result = MatrixHelper.Fill(m1.M, 0); //zeroes
            for (int m = 0; m < m1.M; m++)
            {
                for (int n = 0; n < m1.N; n++)
                {
                    for (int k = 0; k < m1.N; k++)
                    {
                        result[m, n] += m1[m, k]*m2[k, n];
                    }
                }
            }
            return (T) Activator.CreateInstance(typeof (T), result);
        }

        /// <summary>
        ///     Element wise matrix multiplication operator between to matrices A and B
        /// </summary>
        /// <param name="m1">the first matrix A to get the matrix AB </param>
        /// <param name="m2">the second matrix B to get the matrix AB </param>
        /// <returns>the matrix AB from matrices A and B</returns>
        public static T operator ^(SquareMatrix<T> m1, SquareMatrix<T> m2)
        {
            double[,] result = MatrixHelper.Fill(m1.M, 0); //zeroes
            for (int m = 0; m < m1.M; m++)
            {
                for (int n = 0; n < m1.N; n++)
                {
                    result[m, n] = m1[m, n]*m2[m, n];
                }
            }
            return (T) Activator.CreateInstance(typeof (T), result);
        }

        /// <summary>
        ///     Tests whether or not this matrix is equal to another matrix
        /// </summary>
        /// <param name="m1">the first matrix</param>
        /// <param name="m2">the second matrix</param>
        /// <returns>a boolean representing whether or not this matrix is equal to another matrix</returns>
        public static bool operator ==(SquareMatrix<T> m1, SquareMatrix<T> m2)
        {
            if ((object) m1 != null)
            {
                return m1.Equals(m2);
            }
            return null == (object) m2;
        }

        /// <summary>
        ///     Tests whether or not this matrix is not equal to another matrix
        /// </summary>
        /// <param name="m1">the first matrix</param>
        /// <param name="m2">the second matrix</param>
        /// <returns>a boolean representing whether or not this matrix is equal to another matrix</returns>
        public static bool operator !=(SquareMatrix<T> m1, SquareMatrix<T> m2)
        {
            return !(m1 == m2);
        }

        /// <summary>
        ///     The transpose operator. This is equivaluent to calling m.Transpose()
        /// </summary>
        /// <param name="m1">the matrix being transposed (normally self)</param>
        /// <returns>new Matrix3 of the transpose</returns>
        public static T operator !(SquareMatrix<T> m1)
        {
            return m1.Transpose();
        }

        public T Transpose()
        {
            var result = new double[N, M];
            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    result[m, n] = Values[n, m];
                }
            }
            return (T) Activator.CreateInstance(typeof (T), result);
        }

        #endregion

        #region COMPARATORS

        /// <summary>
        ///     Copy method to create a copy of the current matrix
        /// </summary>
        /// <returns>a copy of the matrix</returns>
        public T Copy()
        {
            return (T) Activator.CreateInstance(typeof (T), Values.Copy());
        }

        /// <summary>
        ///     Tests whether or not this matrix is equal to another matrix
        /// </summary>
        /// <param name="obj">the object (matrix) to test against</param>
        /// <returns>a boolean representing whether or not this matrix is equal to another matrix</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is IMatrix))
            {
                return false;
            }
            var mat = (IMatrix) obj;
            if (mat.M != M || mat.N != N)
            {
                return false;
            }

            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    if (mat[m, n] != this[m, n])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        ///     Tests whether or not this matrix is equal to another matrix
        /// </summary>
        /// <param name="obj">the object (matrix) to test against</param>
        /// <returns>a boolean representing whether or not this matrix is equal to another matrix</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        public override string ToString()
        {
            string output = "";
            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    output += this[m, n].ToString("N2") + " ";
                }
                output += "; ";
            }
            return output;
        }

        #region ArrayExtensions

        public double[] Solve(IVector vec)
        {
            double[,] result = Values.Solve(vec.ToColumnMatrix());
            return result.GetColumn(0);
        }

        #endregion
    }
}