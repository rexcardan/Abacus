using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Abacus.Helper;

namespace Abacus.Model3D
{
    public class ObjFileReader
    {
        public List<Vector3> Vertices { get; set; }
        public List<Face> Faces { get; set; }

        public static ObjModel Read(string path)
        {
            string[] lines = FileHelper.ReadLines(path);
            return ParseLines(lines);
        }

        public static ObjModel Read(byte[] bytes)
        {
            string[] lines = FileHelper.ReadLines(bytes);
            return ParseLines(lines);
        }

        public MeshGeometry3D ReadToMesh(string path)
        {
            string[] lines = FileHelper.ReadLines(path);
            return ReadToMesh(lines);
        }

        public MeshGeometry3D ReadToMesh(byte[] bytes)
        {
            string[] lines = FileHelper.ReadLines(bytes);
            return ReadToMesh(lines);
        }

        private static MeshGeometry3D ReadToMesh(string[] lines)
        {
            var geom = new MeshGeometry3D();
            List<Point3D> point3Ds = lines.Where(l => Regex.IsMatch(l, @"^v(\s+-?\d+\.?\d+([eE][-+]?\d+)?){3,3}$"))
                .Select(l => Regex.Split(l, @"\s+", RegexOptions.None).Skip(1).ToArray()) //Skip v
                .Select(nums => new Point3D(double.Parse(nums[0]), double.Parse(nums[1]), double.Parse(nums[2])))
                .ToList();
            geom.Positions = new Point3DCollection(point3Ds);

            var indices = new List<int>();
            lines.Where(l => Regex.IsMatch(l, @"^f(\s\d+(\/+\d+)?){3,3}$"))
                .Select(l => Regex.Split(l, @"\s+", RegexOptions.None).Skip(1).ToArray()) //Skip f
                .Select(i => i.Select(a => Regex.Match(a, @"\d+", RegexOptions.None).Value).ToArray())
                .Select(nums => new[] {int.Parse(nums[0]) - 1, int.Parse(nums[1]) - 1, int.Parse(nums[2]) - 1})
                .ToList()
                .ForEach(indices.AddRange);
            geom.TriangleIndices = new Int32Collection(indices);

            List<Vector3D> normals = lines.Where(l => Regex.IsMatch(l, @"^(vn)(\s+-?\d+\.?\d+([eE][-+]?\d+)?){3,3}$"))
                .Select(l => Regex.Split(l, @"\s+", RegexOptions.None).Skip(1).ToArray()) //Skip v
                .Select(nums => new Vector3D(double.Parse(nums[0]), double.Parse(nums[1]), double.Parse(nums[2])))
                .ToList();
            geom.Normals = new Vector3DCollection(normals);
            return geom;
        }

        public static ObjModel ParseLines(string[] lines)
        {
            List<Vector3> verts = lines.Where(l => Regex.IsMatch(l, @"^v(\s+-?\d+\.?\d+([eE][-+]?\d+)?){3,3}$"))
                .Select(l => Regex.Split(l, @"\s+", RegexOptions.None).Skip(1).ToArray()) //Skip v
                .Select(nums => new Vector3(double.Parse(nums[0]), double.Parse(nums[1]), double.Parse(nums[2])))
                .ToList();
            List<Face> faces = lines.Where(l => Regex.IsMatch(l, @"^f(\s\d+(\/+\d+)?){3,3}$"))
                .Select(l => Regex.Split(l, @"\s+", RegexOptions.None).Skip(1).ToArray()) //Skip f
                .Select(i => i.Select(a => Regex.Match(a, @"\d+", RegexOptions.None).Value).ToArray())
                .Select(nums => new Face(int.Parse(nums[0]) - 1, int.Parse(nums[1]) - 1, int.Parse(nums[2]) - 1))
                .ToList();
            return new ObjModel(verts, faces);
        }
    }
}