using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Abacus.Helper
{
    public class MeshHelper
    {
        /// <summary>
        ///     Save mesh in ASCII WaveFront .OBJ file
        /// </summary>
        public static void ExportMeshAsObj(MeshGeometry3D mesh, string outputPath)
        {
            var lines = new List<string>();
            if (null == mesh)
            {
                return;
            }
            Point3DCollection vertices = mesh.Positions;
            Vector3DCollection normals = mesh.Normals;
            Int32Collection indices = mesh.TriangleIndices;

            // Write the header lines
            lines.Add("#");
            lines.Add("# OBJ file created Abacus lib");
            lines.Add("#");

            // Sequentially write the 3 vertices of the triangle, for each triangle
            for (int i = 0; i < vertices.Count; i++)
            {
                Point3D vertex = vertices[i];
                string vertexString = "v " + vertex.X.ToString(CultureInfo.InvariantCulture) + " ";
                vertexString += vertex.Y.ToString(CultureInfo.InvariantCulture) + " " +
                                vertex.Z.ToString(CultureInfo.InvariantCulture);
                lines.Add(vertexString);
            }

            // Sequentially write the 3 normals of the triangle, for each triangle
            for (int i = 0; i < normals.Count; i++)
            {
                Vector3D normal = normals[i];
                string normalString = "vn " + normal.X.ToString(CultureInfo.InvariantCulture) + " ";
                normalString += normal.Y.ToString(CultureInfo.InvariantCulture) + " " +
                                normal.Z.ToString(CultureInfo.InvariantCulture);
                lines.Add(normalString);
            }

            for (int i = 0; i < indices.Count/3; i++)
            {
                string baseIndex0 = (indices[i*3] + 1).ToString(CultureInfo.InvariantCulture);
                string baseIndex1 = (indices[i*3 + 1] + 1).ToString(CultureInfo.InvariantCulture);
                string baseIndex2 = (indices[i*3 + 2] + 1).ToString(CultureInfo.InvariantCulture);

                string faceString = "f " + baseIndex0 + "//" + baseIndex0 + " " + baseIndex1 + "//" + baseIndex1 + " " +
                                    baseIndex2 + "//" + baseIndex2;
                lines.Add(faceString);
            }
            File.WriteAllLines(outputPath, lines.ToArray());
        }
    }
}