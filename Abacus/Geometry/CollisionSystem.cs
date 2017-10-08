using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Abacus.Geometry
{
    /// <summary>
    /// A two body collision system made from mesh geometry objects (WPF)
    /// </summary>
    public class CollisionSystem
    {
        private MeshCollider _mc;

        /// <summary>
        /// A two body collision system made of mesh geometry objects. The system can detect if there is a collision between 
        /// the two objects (using polygon intererence Opcode)
        /// </summary>
        /// <param name="g1">the first geometry object</param>
        /// <param name="g2">the second geometry object</param>
        public CollisionSystem(MeshGeometry3D g1, MeshGeometry3D g2)
        {
            _mc = new MeshCollider();
            _mc.SetModel1(g1.Positions.SelectMany(p => new double[] { p.X, p.Y, p.Z }).ToArray(), g1.TriangleIndices.ToArray());
            _mc.SetModel2(g2.Positions.SelectMany(p => new double[] { p.X, p.Y, p.Z }).ToArray(), g2.TriangleIndices.ToArray());
        }

        /// <summary>
        /// Returns a bool indicating whether there is a collision between the two objects. 
        /// </summary>
        /// <param name="m1">the 4x4 matrix indicating the transform of the first mesh</param>
        /// <param name="m2">the 4x4 matrix indicating the transform of the second mesh</param>
        /// <returns></returns>
        public bool IsCollisionState(MatrixTransform3D m1, MatrixTransform3D m2)
        {
            var mat1 = m1.Matrix;
            var mat2 = m2.Matrix;
            var tx1 = new double[] {
                mat1.M11, mat1.M12, mat1.M13, mat1.M14,
                mat1.M21, mat1.M22, mat1.M23, mat1.M24,
                mat1.M31, mat1.M32, mat1.M33, mat1.M34,
                0, 0, 0 , mat1.M44 };
            var tx2 = new double[] {
                mat2.M11, mat2.M12, mat2.M13, mat2.M14,
                mat2.M21, mat2.M22, mat2.M23, mat2.M24,
                mat2.M31, mat2.M32, mat2.M33, mat2.M34,
                0, 0, 0 , mat2.M44 };
            return _mc.Collide(tx1, tx2);
        }
    }
}
