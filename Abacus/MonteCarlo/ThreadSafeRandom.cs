using System;
using System.Security.Cryptography;

namespace Abacus.MonteCarlo
{
    /// <summary>
    ///     A random number generator that can validly generate random numbers on multiple threads (for parallel computing)
    /// </summary>
    public class ThreadSafeRandom
    {
        private static readonly RNGCryptoServiceProvider _global =
            new RNGCryptoServiceProvider();

        [ThreadStatic] private static Random _local;

        /// <summary>
        ///     Generates the next random double
        /// </summary>
        /// <returns></returns>
        public static double Next()
        {
            Random inst = _local;
            if (inst == null)
            {
                var buffer = new byte[4];
                _global.GetBytes(buffer);
                _local = inst = new Random(
                    BitConverter.ToInt32(buffer, 0));
            }
            return inst.NextDouble();
        }
    }
}