using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.Geometry;

namespace Abacus.Model3D
{
    /// <summary>
    ///     An object model is a container for vertices, faces, and normals. In one object file, there may be mulitple models
    /// </summary>
    /// <summary>
    ///     An object model is a container for vertices, faces, and normals. In one object file, there may be mulitple models
    /// </summary>
    public class ObjModel
    {
        public ObjModel()
        {
            Normals = new List<Vector3>();
            Vertices = new List<Vector3>();
            Faces = new List<Face>();
        }

        public ObjModel(List<Vector3> vertices, List<Face> faces, List<Vector3> normals = null)
        {
            Vertices = vertices;
            Faces = faces;
            Normals = normals ?? new List<Vector3>();
        }

        public string Name { get; set; }
        public List<Vector3> Vertices { get; set; }
        public List<Vector3> Normals { get; set; }
        public List<Face> Faces { get; set; }

        public List<int> CalculateIndices()
        {
            var ind = new List<int>();
            Faces.ForEach(f =>
            {
                ind.Add(f.Indices[0]);
                ind.Add(f.Indices[1]);
                ind.Add(f.Indices[2]);
            });
            return ind;
        }

        public List<Triangle3D> GetTriangles()
        {
            return
                Faces.Select(f => new Triangle3D(Vertices[f.Indices[0]], Vertices[f.Indices[1]], Vertices[f.Indices[2]]))
                    .ToList();
        }

        public void ClipToBoundingBox(double xMin, double xMax, double yMin, double yMax, double zMin, double zMax)
        {
            var isInBoundsFunc = new Func<Vector3, bool>(vert =>
            {
                if (vert.X <= xMin || vert.X >= xMax)
                {
                    return false;
                }
                if (vert.Y <= yMin || vert.Y >= yMax)
                {
                    return false;
                }
                if (vert.Z <= zMin || vert.Z >= zMax)
                {
                    return false;
                }
                return true;
            });

            int numRemoved = 0;
            var vertexMap = Vertices.Select((v, i) =>
            {
                bool inBounds = isInBoundsFunc(v);
                var map = new
                {
                    Vertex = v,
                    StartIndex = i,
                    EndIndex = inBounds ? i - numRemoved : -1,
                    IsInBounds = inBounds
                };
                if (!inBounds)
                {
                    numRemoved++;
                }
                return map;
            }).ToList();

            //Trim out of bound vertices
            Vertices = vertexMap.Where(v => v.EndIndex != -1).Select(v => v.Vertex).ToList();

            //Remap faces
            int performed = 0;
            foreach (Face f in Faces)
            {
                performed++;
                for (int i = 0; i < f.Indices.Count; i++)
                {
                    var map = vertexMap[f.Indices[i]];
                    f.Indices[i] = map.EndIndex;
                }
            }

            //Trim out faces with out of bound vertices
            Faces = Faces.Where(f => !f.Indices.Contains(-1)).ToList();
        }
    }
}