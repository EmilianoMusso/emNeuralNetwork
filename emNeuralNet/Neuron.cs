using System.Collections.Generic;

namespace emNeuralNet
{
    public class Neuron
    {
        public List<Dendrite> Dendrites { get; set; }
        public double Bias { get; set; }
        public double Delta { get; set; }
        public double Value { get; set; }

        public int DendriteCount
        {
            get
            {
                return Dendrites.Count;
            }
        }

        public Neuron()
        {
            RandomGenerator n = new RandomGenerator();
            this.Bias = n.RandomValue;

            this.Dendrites = new List<Dendrite>();
        }
    }


}
