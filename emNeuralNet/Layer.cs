using System.Collections.Generic;

namespace emNeuralNet
{
    public class Layer
    {
        public List<Neuron> Neurons { get; set; }
        public int NeuronCount
        {
            get
            {
                return Neurons.Count;
            }
        }

        public Layer() { }

        public Layer(int numNeurons)
        {
            Neurons = new List<Neuron>(numNeurons);
        }
    }
}
