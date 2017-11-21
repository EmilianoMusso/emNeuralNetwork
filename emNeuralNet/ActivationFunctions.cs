using System;

namespace emNeuralNet
{
    public class ActivationFunctions
    {
        public enum Activation
        {
            SIGMOID = 0,
            TANH = 1,
            STEP = 2,
            IDENTITY = 3
        }

        public static double Activate(Activation a, double input)
        {
            double retVal = 0;

            switch (a)
            {
                case Activation.SIGMOID:
                    retVal = Sigmoid(input);
                    break;

                case Activation.TANH:
                    retVal = TanH(input);
                    break;

                case Activation.STEP:
                    retVal = Step(input);
                    break;

                case Activation.IDENTITY:
                    retVal = input;
                    break;
            }

            return retVal;
        }

        // ***********************************************************************************************************************

        private static double Sigmoid(double x)
        {
            if (x < -45.0) return 0.0;
            else if (x > 45.0) return 1.0;
            else return 1.0 / (1.0 + Math.Exp(-x));
        }

        private static double TanH(double x)
        {
            if (x < -45.0) return -1.0;
            else if (x > 45.0) return 1.0;
            else return Math.Tanh(x);
        }

        public static double Step(double x)
        {
            if (x < 0.0) return 0.0;
            else return 1.0;
        }
    }
}
