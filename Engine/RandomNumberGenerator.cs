using System;
using System.Security.Cryptography;

namespace Engine
{
    public static class RandomNumberGenerator
    {
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        public static int GetRandNumberBetween(int minVal, int maxVal)
        {
            byte[] randomNumber = new byte[1];

            _generator.GetBytes(randomNumber);

            double asciiValueOfRandCharacter = Convert.ToDouble(randomNumber[0]);

            double multiplier = Math.Max(0, (asciiValueOfRandCharacter / 255d) - 0.00000000001d);
            int range = maxVal - minVal + 1;
            double randValueInRange = Math.Floor(multiplier * range);
            return (int)(minVal + randValueInRange);
        }
    }
}
