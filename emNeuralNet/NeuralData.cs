using System;
using System.Collections.Generic;

namespace emNeuralNet
{
    public class NeuralData: List<double>
    {
        public enum DATATYPE
        {
            INPUT = 0,
            OUTPUT = 1
        }

        public NeuralData() { }

        public NeuralData(params double[] data)
        {
            this.AddRange(data);
        }

        /// <summary>
        /// Convert a number to its binary representation, adding leading zeros
        /// </summary>
        /// <param name="number">The value to convert to binary</param>
        /// <param name="neuronCount">The number of neurons to host that number, i.e. the leading zeros + the binary number length</param>
        /// <returns></returns>
        public static double[] ConvertToBinary(long number, int neuronCount)
        {
            string bin = Convert.ToString(number, 2).PadLeft(neuronCount, '0');
            return StringToNeural(bin);
        }

        /// <summary>
        /// Convert a character to its binary representation, adding leading zeros
        /// </summary>
        /// <param name="character"></param>
        /// <param name="neuronCount"></param>
        /// <returns></returns>
        public static double[] ConvertToBinary(char character, int neuronCount)
        {
            int charVal = (int)character;

            string bin = Convert.ToString(charVal, 2).PadLeft(neuronCount, '0');
            return StringToNeural(bin);
        }

        public static int BinaryToInt(double[] input)
        {
            string binStr = "";
            foreach(var d in input)
            {
                binStr += Math.Round(d);
            }

            return Convert.ToInt32(binStr, 2);
        }

        /// <summary>
        /// Internal function to return an array of double from an input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static double[] StringToNeural(string input)
        {
            var nd = new NeuralData();

            foreach (var c in input.ToCharArray())
            {
                string ch = c.ToString();
                double d;
                double.TryParse(ch, out d);

                nd.Add(d);
            }

            return nd.ToArray();
        }
    }
}
