using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace JasonSoft.Math
{
    public static class MathExtension
    {
        public static Int32 GetAge(this DateTime source)
        {
            DateTime now = DateTime.Now;
            if(source > now) throw new ArgumentException("source datetime can't bigger than right now.");
            return DateTime.Now.Subtract(source).Days/365;
        }

        /// <summary>
        /// Determines whether the specified value is an even number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     <c>true</c> if the specified value is even; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEven(this int value)
        {
            return ((value & 1) == 0);
        }

        /// <summary>
        /// Determines whether the specified value is an odd number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     <c>true</c> if the specified value is odd; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOdd(this int value)
        {
            return ((value & 1) == 1);
        }

        /// <summary>
        /// Generates a random number
        /// </summary>
        /// <param name="noZeros"></param>
        /// <returns></returns>
        public static int Random(this bool noZeros)
        {
            byte[] random = new Byte[1];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            if (noZeros)
                rng.GetNonZeroBytes(random);
            else
                rng.GetBytes(random);

            return random[0];
        }

        /// <summary>
        /// Generates a random number with an upper bound
        /// </summary>
        /// <param name="high">The high.</param>
        /// <returns></returns>
        public static int Random(this int high)
        {
            byte[] random = new Byte[4];
            new RNGCryptoServiceProvider().GetBytes(random);
            int randomNumber = BitConverter.ToInt32(random, 0);
            
            return System.Math.Abs(randomNumber % high);
        }

        /// <summary>
        /// Generates a random number between the specified bounds
        /// </summary>
        /// <param name="low">The low.</param>
        /// <param name="high">The high.</param>
        /// <returns></returns>
        public static int Random(this int low, int high)
        {
            return new Random(true.Random()).Next(low, high);
        }

        public static Int32 Random(this Int32 lowest, Int32 highest, List<Int32> exclude)
        {
            Int32 result = lowest.Random(highest);

            if (!exclude.IsNullOrEmpty())
            {
                while (exclude.Contains(result))
                {
                    result = lowest.Random(highest);
                }
            }

            return result;
        }

        /// <summary>
        /// Generates a random double
        /// </summary>
        /// <returns></returns>
        public static double Random()
        {
            return new Random().NextDouble();
        }


    }
}
