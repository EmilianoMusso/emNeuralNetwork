namespace emNeuralNet
{
    public class Dendrite
    {
        public double Weight { get; set; }

        public Dendrite()
        {
            RandomGenerator n = new RandomGenerator();
            this.Weight = n.RandomValue;
        }
    }

}
