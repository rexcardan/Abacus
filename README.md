# Abacus ![alt text](https://dl.dropboxusercontent.com/u/7527702/CodeDeCardan_32x32png.png "Code De Cardan Logo")
A lightweight but powerful C# Math library for working with Matrices, Vectors, and 3D geometry

On Nuget as [Abacus.Rex] (https://www.nuget.org/packages/Abacus.Rex/)

##Vector Types
###Vector2
###Vector3
###Vector4
##Matrix Types
###Matrix3 (3x3)
###Matrix4 (4x4)

```cs
            var matrix4 = Matrix4.Identity;

            var m = matrix4.M; //number of rows = 4
            var n = matrix4.N; //number of columns = 4;
```
![alt text](http://mathurl.com/jksf4gy.png "Identity matrix")

###Matrix Extensions to Double[,]
You don't even have to use Matrix types. Just create a double [,] and you are good to go!

```cs
            var matrix4 = Matrix4.Identity;

            var m = matrix4.M; //number of rows = 4
      

            var identity4 = new double[4, 4]
            {
                {1,0,0,0 },
                {0,1,0,0 },
                {0,0,1,0 },
                {0,0,0,1 }
            };

            var secondMatrix = new double[4, 4]
            {
                {2,4,2,4 },
                {3,5,4.7,4 },
                {2,1,10,100 },
                {10,77,66,55 }
            };

            //Matrix Operations
            var multiply = identity4.Multiply(2);
            var elementWise = identity4.ElementWiseMultiply(secondMatrix);
            var determinant = identity4.Determinant();
            var divide = identity4.Divide(3);
            var invert = identity4.Inverse();
            var transpose = identity4.Transpose();

            //Row/Col operations
            var col1 = identity4.GetColumn(1); //As double[]
            var row1 = identity4.GetRow(1); //As double[]
            var addedRow = identity4.InsertRow(new double[] { 2, 2, 2, 2 }, rowPos: 3); //Inserts a new row in index 3
```