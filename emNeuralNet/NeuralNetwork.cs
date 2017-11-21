using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static emNeuralNet.ActivationFunctions;

namespace emNeuralNet
{
    [Serializable]
    public class NeuralNetwork
    {
        public string Name { get; set; }
        public List<Layer> Layers { get; set; }
        public double LearningRate { get; set; }
        public Activation Activation { get; set; }
        public long TrainingRounds { get; set; }

        public int InputNeuronsCount
        {
            get
            {
                return Layers[0].NeuronCount;
            }
        }

        public int OutputNeuronsCount
        {
            get
            {
                return Layers[LayerCount - 1].NeuronCount;
            }
        }

        public int LayerCount
        {
            get
            {
                return Layers.Count;
            }
        }

        public NeuralNetwork() { }

        /// <summary>
        /// NeuralNetwork constructor
        /// </summary>
        /// <param name="learningRate">The learning rate</param>
        /// <param name="layers">Array of int, each value represents the number of neuron for that layer</param>
        /// <param name="activation">The activation function to be used by the network (if not specified, defaults to SIGMOID)</param>
        /// <param name="name">The network name (if not given, a GUID will be assigned)</param>
        public NeuralNetwork(double learningRate, int[] layers, Activation activation = Activation.SIGMOID, string name = "")
        {
            if (layers.Length < 2) return;

            this.LearningRate   = learningRate;
            this.Layers         = new List<Layer>();
            this.Activation     = activation;
            this.TrainingRounds = 0;

            if (name != "")
            {
                this.Name = name;
            }
            else
            {
                this.Name = "NeuralNetWork-" + Guid.NewGuid().ToString();
            }

            for(int l = 0; l < layers.Length; l++)
            {
                Layer layer = new Layer(layers[l]);
                this.Layers.Add(layer);

                for (int n = 0; n < layers[l]; n++)
                    layer.Neurons.Add(new Neuron());

                layer.Neurons.ForEach((nn) =>
                {
                    if (l == 0)
                        nn.Bias = 0;
                    else
                        for (int d = 0; d < layers[l - 1]; d++)
                            nn.Dendrites.Add(new Dendrite());
                });
            }
        }

        /// <summary>
        /// Executes the neural network
        /// </summary>
        /// <param name="input">Input data</param>
        /// <returns></returns>
        public NeuralData Run(NeuralData input)
        {
            if (input.Count != this.Layers[0].NeuronCount) return null;

            for (int l = 0; l < Layers.Count; l++)
            {
                Layer layer = Layers[l];

                for (int n = 0; n < layer.Neurons.Count; n++)
                {
                    Neuron neuron = layer.Neurons[n];

                    if (l == 0)
                        neuron.Value = input[n];
                    else
                    {
                        neuron.Value = 0;
                        for (int np = 0; np < this.Layers[l - 1].Neurons.Count; np++)
                            neuron.Value = neuron.Value + this.Layers[l - 1].Neurons[np].Value * neuron.Dendrites[np].Weight;

                        neuron.Value = Activate(this.Activation, neuron.Value + neuron.Bias);                        
                    }
                }
            }

            Layer last = this.Layers[this.Layers.Count - 1];
            int numOutput = last.Neurons.Count ;
            double[] output = new double[numOutput];
            for (int i = 0; i < last.Neurons.Count; i++)
                output[i] = last.Neurons[i].Value;

            return new NeuralData(output);
        }

        /// <summary>
        /// Train neural network
        /// </summary>
        /// <param name="input">Input data</param>
        /// <param name="output">Output data</param>
        /// <param name="rounds">Optional number of rounds (defaults to 1)</param>
        /// <returns></returns>
        public bool Train(NeuralData input, NeuralData output, long rounds = 1)
        {
            if ((input.Count != this.Layers[0].Neurons.Count) || (output.Count != this.Layers[this.Layers.Count - 1].Neurons.Count)) return false;

            for (long r = 0; r < rounds; r++)
            {
                Run(input);

                for (int i = 0; i < this.Layers[this.Layers.Count - 1].Neurons.Count; i++)
                {
                    Neuron neuron = this.Layers[this.Layers.Count - 1].Neurons[i];

                    neuron.Delta = neuron.Value * (1 - neuron.Value) * (output[i] - neuron.Value);

                    for (int j = this.Layers.Count - 2; j >= 1; j--)
                    {
                        for (int k = 0; k < this.Layers[j].Neurons.Count; k++)
                        {
                            Neuron n = this.Layers[j].Neurons[k];

                            n.Delta = n.Value *
                                        (1 - n.Value) *
                                        this.Layers[j + 1].Neurons[i].Dendrites[k].Weight *
                                        this.Layers[j + 1].Neurons[i].Delta;
                        }
                    }
                }

                for (int i = this.Layers.Count - 1; i >= 1; i--)
                {
                    for (int j = 0; j < this.Layers[i].Neurons.Count; j++)
                    {
                        Neuron n = this.Layers[i].Neurons[j];
                        n.Bias = n.Bias + (this.LearningRate * n.Delta);

                        for (int k = 0; k < n.Dendrites.Count; k++)
                            n.Dendrites[k].Weight = n.Dendrites[k].Weight + (this.LearningRate * this.Layers[i - 1].Neurons[k].Value * n.Delta);
                    }
                }

                ++this.TrainingRounds;
            }

            return true;
        }

        // **************************************************************************************************************************************************

        /// <summary>
        /// Returns a text representation of the NeuralNetwork current state
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder("Name: " + this.Name);
            sb.AppendLine();

            for (int l = 0; l < this.Layers.Count; l++)
            {
                sb.AppendLine("\tLayer " + l.ToString() + " (" + this.Layers[l].NeuronCount.ToString() + " neurons)");

                for (int n = 0; n < this.Layers[l].NeuronCount; n++)
                {
                    sb.AppendLine("\t\tNeuron " + n.ToString() + " (" + this.Layers[l].Neurons[n].DendriteCount.ToString() + " dendrites)");
                    sb.AppendLine("\t\t\tBias: " + this.Layers[l].Neurons[n].Bias.ToString());
                    sb.AppendLine("\t\t\tDelta: " + this.Layers[l].Neurons[n].Delta.ToString());
                    sb.AppendLine("\t\t\tValue: " + this.Layers[l].Neurons[n].Value.ToString());
                    sb.AppendLine("\t\t\tDendrites");

                    for (int d = 0; d < this.Layers[l].Neurons[n].DendriteCount; d++)
                    {
                        sb.AppendLine("\t\t\t\tDendrite " + d.ToString() + " weight: " + this.Layers[l].Neurons[n].Dendrites[d].Weight);
                    }
                }
            }

            return sb.ToString();
        }


    }
}
