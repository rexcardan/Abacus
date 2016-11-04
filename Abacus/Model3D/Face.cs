using System.Collections.Generic;

namespace Abacus.Model3D
{
    public class Face
    {
        public Face()
        {
            Indices = new List<int>();
        }

        public Face(int index1, int index2, int index3)
            : this()
        {
            Indices.Add(index1);
            Indices.Add(index2);
            Indices.Add(index3);
        }

        public List<int> Indices { get; set; }
    }
}