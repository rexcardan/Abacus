using System;

namespace Abacus.Exceptions
{
    public class NonConvergenceException : Exception
    {
        public NonConvergenceException(int iterations)
            : base(
                string.Format("Could not converge within {0} iterations", iterations))
        {
        }
    }
}