namespace Abacus.Helper
{
    public class IndexHelper
    {
        /// <summary>
        ///     Returns the position x y z in a lattice from an index value. This function assumes that the index starts
        ///     at zero at the point 0,0,0 and increases first in the X direction, second in the y direction, and third in the z
        ///     direction.
        /// </summary>
        /// <param name="index">the index to find the x, y, z</param>
        /// <param name="dimensionX">the number of elements along the x direction</param>
        /// <param name="dimensionY">the number of elements along the y direction</param>
        /// <returns>a vector3 to the point in the lattice represented by the index i</returns>
        public static Vector3 IndexToLatticeXyz(int index, int dimensionX, int dimensionY)
        {
            int z = index/(dimensionX*dimensionY);
            int y = (index%(dimensionX*dimensionY))/(dimensionY);
            int x = (index%(dimensionX*dimensionY))%(dimensionX);
            return new Vector3(x, y, z);
        }

        /// <summary>
        ///     Returns the position x y in a lattice from an index value. This function assumes that the index starts
        ///     at zero at the point 0,0 and increases first in the X direction, and lastly in the y direction
        /// </summary>
        /// <param name="index">the index to find the x, y</param>
        /// <param name="dimensionX">the number of elements along the x direction</param>
        /// <returns>a vector2 to the point in the lattice represented by the index i</returns>
        public static Vector2 IndexToLatticeXy(int index, int dimensionX)
        {
            int x = (index%dimensionX)*1;
            int y = index/dimensionX;
            return new Vector2(x, y);
        }

        /// <summary>
        ///     Returns the index of an element in a 3D lattice structure. This function assumes that the index starts
        ///     at zero at the point 0,0,0 and increases first in the X direction, second in the y direction, and third in the z
        ///     direction.
        /// </summary>
        /// <param name="x">the discrete X position lattice</param>
        /// <param name="y">the discrete Y position in the lattice</param>
        /// <param name="z">the discrete Z position in the lattice</param>
        /// <param name="dimensionX">the number of elements along the x direction</param>
        /// <param name="dimensionY">the number of elements along the y direction</param>
        /// <returns>the index of element at position x,y,z in the lattice</returns>
        public static int LatticeXYZToIndex(int x, int y, int z, int dimensionX, int dimensionY)
        {
            return x + (y*dimensionX) + (z*(dimensionX*dimensionY));
        }

        /// <summary>
        ///     Returns the index of an element in a 2D lattice structure. This function assumes that the index starts
        ///     at zero at the point 0,0 and increases first in the X direction, and lastly in the y direction
        /// </summary>
        /// <param name="x">the discrete X position lattice</param>
        /// <param name="y">the discrete Y position in the lattice</param>
        /// <param name="dimensionX">the number of elements along the x direction</param>
        /// <param name="dimensionY">the number of elements along the y direction</param>
        /// <returns>the index of element at position x,y,z in the lattice</returns>
        public static int LatticeXYToIndex(int x, int y, int dimensionX)
        {
            return x + (y*dimensionX);
        }
    }
}