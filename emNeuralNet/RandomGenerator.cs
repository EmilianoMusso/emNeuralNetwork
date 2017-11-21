using System;
using System.Security.Cryptography;

namespace emNeuralNet
{
    public class RandomGenerator
    {
        public double RandomValue { get; set; }

        public RandomGenerator()
        {
            using (RNGCryptoServiceProvider p = new RNGCryptoServiceProvider())
            {
                Random r = new Random(p.GetHashCode());
                this.RandomValue = r.NextDouble();
            }
        }

    }
}
