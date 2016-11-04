using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.Geometry;
using Abacus.Helper;
using Abacus.Interface;
using A = Abacus.Helper.AngleHelper;

namespace Abacus
{
    public class Matrix4 : SquareMatrix<Matrix4>, IMatrix
    {
        #region CONSTRUCTORS

        public Matrix4()
            : base(MatrixHelper.Fill(4, 0))
        {
        }

        /// <summary>
        ///     Double array constructor. Initializes matrix to contain the values of a double array. The double
        ///     array should be double[4,4] for Matrix3 methods to work properly
        /// </summary>
        /// <param name="values"></param>
        public Matrix4(double[,] values) : base(values, 4)
        {
        }

        /// <summary>
        ///     Vector constructor for 4 x 4 matrix. Initializes rows as input vectors.
        /// </summary>
        /// <param name="row1">Vector4 representing the first row of the matrix</param>
        /// <param name="row2">Vector4 representing the second row of the matrix</param>
        /// <param name="row3">Vector4 representing the third row of the matrix</param>
        /// ///
        /// <param name="row4">Vector4 representing the third row of the matrix</param>
        public Matrix4(Vector4 row1, Vector4 row2, Vector4 row3, Vector4 row4)
            : base(new double[4, 4]
            {
                {row1[0], row1[1], row1[2], row1[3]},
                {row2[0], row2[1], row2[2], row2[3]},
                {row3[0], row3[1], row3[2], row3[3]},
                {row4[0], row4[1], row4[2], row4[3]}
            })
        {
        }

        /// <summary>
        ///     Create a transform matrix by using a 3 x 3 rotation matrix and a vector translation
        /// </summary>
        /// <param name="matrix3"></param>
        /// <param name="vector3"></param>
        public Matrix4(Matrix3 m3, Vector3 v3)
            : base(new double[4, 4])
        {
            for (int m = 0; m < 3; m++)
            {
                for (int n = 0; n < 3; n++)
                {
                    this[m, n] = m3[m, n];
                }
            }
            this[0, 3] = v3.X;
            this[1, 3] = v3.Y;
            this[2, 3] = v3.Z;
            this[3, 3] = 1;
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        ///     The Matrix vector product operator.
        /// </summary>
        /// <param name="m1">matrix to be multiplied</param>
        /// <param name="v1">vector to be multiplied</param>
        /// <returns>a new vector containing the multiplied values</returns>
        public static Vector4 operator *(Matrix4 m1, Vector4 v1)
        {
            if (m1 == null) throw new ArgumentNullException("m1");
            return v1*m1;
        }

        /// <summary>
        ///     The Matrix vector product operator.
        /// </summary>
        /// <param name="m1">matrix to be multiplied</param>
        /// <param name="v1">vector to be multiplied</param>
        /// <returns>a new vector containing the multiplied values</returns>
        public static Vector4 operator *(Vector4 v1, Matrix4 m1)
        {
            return new Vector4(
                (m1[0, 0]*v1[0] + m1[0, 1]*v1[1] + m1[0, 2]*v1[2] + m1[0, 3]*v1[3]),
                (m1[1, 0]*v1[0] + m1[1, 1]*v1[1] + m1[1, 2]*v1[2] + m1[1, 3]*v1[3]),
                (m1[2, 0]*v1[0] + m1[2, 1]*v1[1] + m1[2, 2]*v1[2] + m1[2, 3]*v1[3]),
                (m1[3, 0]*v1[0] + m1[3, 1]*v1[1] + m1[3, 2]*v1[2] + m1[3, 3]*v1[3])
                );
        }

        #endregion

        public static Matrix4 Identity
        {
            get { return new Matrix4(MatrixHelper.Identity(4)); }
        }

        public double M11
        {
            get { return Values[0, 0]; }
            set { Values[0, 0] = value; }
        }

        public double M12
        {
            get { return Values[0, 1]; }
            set { Values[0, 1] = value; }
        }

        public double M13
        {
            get { return Values[0, 2]; }
            set { Values[0, 2] = value; }
        }

        public double M14
        {
            get { return Values[0, 3]; }
            set { Values[0, 3] = value; }
        }

        public double M21
        {
            get { return Values[1, 0]; }
            set { Values[1, 0] = value; }
        }

        public double M22
        {
            get { return Values[1, 1]; }
            set { Values[1, 1] = value; }
        }

        public double M23
        {
            get { return Values[1, 2]; }
            set { Values[1, 2] = value; }
        }

        public double M24
        {
            get { return Values[1, 3]; }
            set { Values[1, 3] = value; }
        }

        public double M31
        {
            get { return Values[2, 0]; }
            set { Values[2, 0] = value; }
        }

        public double M32
        {
            get { return Values[2, 1]; }
            set { Values[2, 1] = value; }
        }

        public double M33
        {
            get { return Values[2, 2]; }
            set { Values[2, 2] = value; }
        }

        public double M34
        {
            get { return Values[2, 3]; }
            set { Values[2, 3] = value; }
        }

        public double M41
        {
            get { return Values[3, 0]; }
            set { Values[3, 0] = value; }
        }

        public double M42
        {
            get { return Values[3, 1]; }
            set { Values[3, 1] = value; }
        }

        public double M43
        {
            get { return Values[3, 2]; }
            set { Values[3, 2] = value; }
        }

        public double M44
        {
            get { return Values[3, 3]; }
            set { Values[3, 3] = value; }
        }

        public static Matrix4 FromTransform(Vector3 rotations, Vector3 translations,
            DegreeType degType = DegreeType.Radians)
        {
            Vector3 r = rotations;
            Vector3 t = translations;

            Matrix4 zRot = ZRotationMat(r.Z, degType);
            Matrix4 yRot = YRotationMat(r.Y, degType);
            Matrix4 xRot = XRotationMat(r.X, degType);
            Matrix4 tx = TranslationMat(t);

            return zRot*yRot*xRot*tx;
        }

        public static Matrix4 XRotationMat(double angle, DegreeType degType = DegreeType.Radians)
        {
            double w = A.ToRadian(angle, degType);
            return new Matrix4(new double[4, 4]
            {
                {1, 0, 0, 0},
                {0, A.Cos(w), -A.Sin(w), 0},
                {0, A.Sin(w), A.Cos(w), 0},
                {0, 0, 0, 1}
            });
        }

        public static Matrix4 YRotationMat(double angle, DegreeType degType = DegreeType.Radians)
        {
            double t = A.ToRadian(angle, degType);
            return new Matrix4(new double[4, 4]
            {
                {A.Cos(t), 0, A.Sin(t), 0},
                {0, 1, 0, 0},
                {-A.Sin(t), 0, A.Cos(t), 0},
                {0, 0, 0, 1}
            });
        }

        public static Matrix4 ZRotationMat(double angle, DegreeType degType = DegreeType.Radians)
        {
            double p = A.ToRadian(angle, degType);
            return new Matrix4(new double[4, 4]
            {
                {A.Cos(p), -A.Sin(p), 0, 0},
                {A.Sin(p), A.Cos(p), 0, 0},
                {0, 0, 1, 0},
                {0, 0, 0, 1}
            });
        }

        public static Matrix4 TranslationMat(Vector3 v)
        {
            return new Matrix4(new double[4, 4]
            {
                {1, 0, 0, v.X},
                {0, 1, 0, v.Y},
                {0, 0, 1, v.Z},
                {0, 0, 0, 1}
            });
        }

        public static Matrix4 TranslationMat(double x, double y, double z)
        {
            return TranslationMat(new Vector3(x, y, z));
        }

        public Matrix4 Translate(Vector3 v)
        {
            return this*TranslationMat(v);
        }

        public Matrix4 Translate(double x, double y, double z)
        {
            return this*TranslationMat(x, y, z);
        }

        public Matrix4 Scale(double scale)
        {
            return (Identity*scale)*this;
        }

        public Matrix4 Scale(double xscale, double yscale, double zscale)
        {
            Matrix4 identity = Identity;
            identity.M11 *= xscale;
            identity.M22 *= yscale;
            identity.M33 *= zscale;
            return identity*this;
        }

        public Matrix4 RotX(double angle, DegreeType degType = DegreeType.Radians)
        {
            Matrix4 xRot = XRotationMat(angle, degType);
            Matrix4 result = this*xRot;
            double[,] values = (this*XRotationMat(angle, degType)).Values;
            return new Matrix4(values);
        }

        public Matrix4 RotY(double angle, DegreeType degType = DegreeType.Radians)
        {
            double[,] values = (this*YRotationMat(angle, degType)).Values;
            return new Matrix4(values);
        }

        public Matrix4 RotZ(double angle, DegreeType degType = DegreeType.Radians)
        {
            double[,] values = (this*ZRotationMat(angle, degType)).Values;
            return new Matrix4(values);
        }

        public IEnumerable<Vector4> Transform(IEnumerable<Vector4> points)
        {
            var txed = new List<Vector4>();
            foreach (Vector4 p in points)
            {
                Vector4 tx = this*p;
                txed.Add(tx);
            }
            return txed;
        }

        public IEnumerable<Vector3> Transform(IEnumerable<Vector3> points)
        {
            return Transform(points.ToArray());
        }

        public IEnumerable<Vector3> Transform(params Vector3[] points)
        {
            var txed = new List<Vector4>();
            foreach (Vector3 p in points)
            {
                Vector4 tx = this*p.Homogeneous;
                txed.Add(tx);
            }
            return txed.Select(t => new Vector3(t.X, t.Y, t.Z)).ToList();
        }

        public Matrix4 Inverse()
        {
            return new Matrix4(Values.Inverse());
        }
    }
}