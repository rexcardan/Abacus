using System;

namespace Abacus.Exceptions
{
    public class InvalidSizeException : Exception
    {
        public InvalidSizeException(int sizeExpected, int sizeActual)
            : base(string.Format("Invalid size. Expected {0} length. Actual was {1}", sizeExpected, sizeActual))
        {
        }
    }
}