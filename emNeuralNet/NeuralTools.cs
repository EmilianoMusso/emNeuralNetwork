using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace emNeuralNet
{
    public static class NeuralTools
    {
        /// <summary>
        /// Save the NeuralNetwork status to a xml file. If the path is not indicated as argument, it will be asked for
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool Save(this NeuralNetwork nn, string path = "")
        {
            try
            {
                string pathFile = path;

                if (pathFile == "")
                {
                    var sd = new SaveFileDialog();
                    sd.Title = "Save " + nn.Name;
                    sd.Filter = "XML File (*.xml)|*.xml";
                    if (sd.ShowDialog() == DialogResult.OK) pathFile = sd.FileName;
                }

                using (var writer = new StreamWriter(pathFile))
                {
                    var serializer = new XmlSerializer(nn.GetType());
                    serializer.Serialize(writer, nn);
                    writer.Flush();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Load and initialize a NeuralNetwork object from a xml file
        /// </summary>
        /// <param name="pathFile">The xml file path in which neural network previous state is stored</param>
        /// <returns></returns>
        public static NeuralNetwork Load(this NeuralNetwork nn, string pathFile)
        {
            try
            {
                using (var stream = File.OpenRead(pathFile))
                {
                    var serializer = new XmlSerializer(typeof(NeuralNetwork));
                    return serializer.Deserialize(stream) as NeuralNetwork;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Create a 1600x1200 JPG file, containing a graphical representation of the neural network
        /// </summary>
        /// <returns></returns>
        public static Image GetImage(this NeuralNetwork nn, int width, int height)
        {
            int neuronSize = 30;
            int spacingX = 45;
            int spacingY = 120;

            var i = new Bitmap(width, height);

            var g = Graphics.FromImage(i);
            g.FillRectangle(Brushes.White, 0, 0, width, height);

            int _x = 0;
            int _y = height - spacingY;

            int lidx = 0;

            nn.Layers.ForEach(l =>
            {
                _x = (width / 2) - ((l.NeuronCount * spacingX) / 2);

                l.Neurons.ForEach(n =>
                {
                    g.DrawEllipse(Pens.Black, _x, _y, neuronSize, neuronSize);

                    n.Dendrites.ForEach(d =>
                    {
                        int _pre_x = (width / 2) - ((nn.Layers[lidx - 1].NeuronCount * spacingX) / 2);

                        for (int idx = 0; idx < nn.Layers[lidx - 1].NeuronCount; idx++)
                        {
                            g.DrawLine(Pens.LightGray, _x + neuronSize / 2, _y + neuronSize, _pre_x + neuronSize / 2, _y + spacingY);

                            _pre_x += spacingX;
                        }
                    });

                    _x += spacingX;
                });

                _y -= spacingY;
                ++lidx;
            });

            return i;
        }

        /// <summary>
        /// Save a 1600x1200 JPG file, containing a graphical representation of the neural network
        /// </summary>
        /// <param name="pathFile">The path in which to save the jpg file</param>
        public static void SaveImage(this NeuralNetwork nn, string pathFile, int width, int height)
        {
            nn.GetImage(width, height).Save(pathFile);
        }

        /// <summary>
        /// Extension to NeuralData for quick value conversion toward int
        /// </summary>
        /// <param name="nd"></param>
        /// <returns></returns>
        public static int BinaryToInt(this NeuralData nd)
        {
            return NeuralData.BinaryToInt(nd.ToArray());
        }

        /// <summary>
        /// Extenstion to NeuralData for quick conversion and initialization of integer input value
        /// </summary>
        /// <param name="nw"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static NeuralData InputDataInt(this NeuralNetwork nw, long number)
        {
            return new NeuralData(NeuralData.ConvertToBinary(number, nw.InputNeuronsCount));
        }

        /// <summary>
        /// Extenstion to NeuralData for quick conversion and initialization of integer output value
        /// </summary>
        /// <param name="nw"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static NeuralData OutputDataInt(this NeuralNetwork nw, long number)
        {
            return new NeuralData(NeuralData.ConvertToBinary(number, nw.OutputNeuronsCount));
        }
    }
}
