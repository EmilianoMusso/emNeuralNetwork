using emNeuralNet;
using System.Windows.Forms;

using static emNeuralNet.ActivationFunctions;
using nn = emNeuralNet.NeuralNetwork;
using nd = emNeuralNet.NeuralData;


namespace TestNeural
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            nn nw = new nn(1.25, new int[]{ 2, 2, 1 }, Activation.SIGMOID, "TestNet");
            //nw = nw.Load(@"c:\tmp\test_multip.xml");

            //nw.SaveImage(@"c:\tmp\nn.jpg");

            for (long i = 0; i < 500000; i++)
            {
                nd outx = nw.OutputDataInt(0);
                nd inpx = new nd(0, 0);
                nw.Train(inpx, outx, 5);

                outx = nw.OutputDataInt(1);
                inpx = new nd(0, 1);
                nw.Train(inpx, outx, 5);

                outx = nw.OutputDataInt(1);
                inpx = new nd(1, 0);
                nw.Train(inpx, outx, 5);

                outx = nw.OutputDataInt(0);
                inpx = new nd(1,1);
                nw.Train(inpx, outx, 5);
            }

            MessageBox.Show("Training rounds so far: " + nw.TrainingRounds.ToString());

            // *****************************************************************

            nd outp = nw.Run( new nd(1, 1) );
            MessageBox.Show(outp.BinaryToInt().ToString());


            //nw.Save();
        }
    }
}
