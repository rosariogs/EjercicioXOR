using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace perceptronXOR
{
    class Program
    {
        static void Main(string[] args)
        {
            train();
        }

        class sigmoid
        {
            public static double output(double x)
            {
                return 1.0 / (1.0 + Math.Exp(-x));
            }

            public static double derivative (double x)
            {
                return x * (1-x);
            }
        }

        class Neuron
        {
            public double[] inputs = new double[2];
            public double[] weights = new double[2];
            public double error;

            private double biasWeight;
            private Random r = new Random();

            public double output
            {
                get {return sigmoid.output(weights[0] * inputs[0] + weights[1] * inputs[1] + biasWeight); }
            }

            public void randomizeWeights()
            {
                weights[0] = r.NextDouble();
                weights[1] = r.NextDouble();
                biasWeight = r.NextDouble();
            }

            public void adjustWeights()
            {
                weights[0] += error * inputs[0];
                weights[1] += error * inputs[1];
                biasWeight += error;
            }

        }

        private static void train()
        {
            //the input values - los valores de entrada
            double[,] inputs =
            {
                { 0, 0},
                { 0, 1},
                { 1, 0},
                { 1, 1}
            };

            //desired results - resultados deseados
            double[] results = {0, 1, 1, 0};

            //creating the neurons - creacion de neuronas
            Neuron hiddenNeuron1 = new Neuron();
            Neuron hiddenNeuron2 = new Neuron();
            Neuron outputNeuron = new Neuron();

            // random weights - pesos aleatorios
            hiddenNeuron1.randomizeWeights();
            hiddenNeuron2.randomizeWeights();
            outputNeuron.randomizeWeights();

            int epoch = 0;

        Retry:
            epoch++;
            for (int i = 0; i < 4; i++)
            {
                // 1) forward propagation (calculates output) - propagación directa (calcula la salida)
                hiddenNeuron1.inputs = new double[] {inputs[i, 0], inputs[i, 1]};
                hiddenNeuron2.inputs = new double[] {inputs[i, 0], inputs[i, 1]};

                outputNeuron.inputs = new double[] {hiddenNeuron1.output, hiddenNeuron2.output};

                Console.writeLine("{0} xor {1} = {2}", inputs[i, 0], inputs[i, 1], outputNeuron.output);

                // 2) back propagation (adjusts weigths) - retropropagación (ajusta pesos)
                // adjusts the weight of the output neuron, based on its error - ajusta el peso de la neurona de salida, en función de su error
                outputNeuron.error = sigmoid.derivative(outputNeuron.output) * (results[i] - outputNeuron.output);
                outputNeuron.adjustWeights();

                //the adjusts the hidden neurons weights, based on their errors - ajusta los pesos de las neuronas ocultas, en función de sus errores
                hiddenNeuron1.error = sigmoid.derivative(hiddenNeuron1.output) * outputNeuron.error * outputNeuron.weights[0];
                hiddenNeuron2.error = sigmoid.derivative(hiddenNeuron2.output) * outputNeuron.error * outputNeuron.weights[1];

                hiddenNeuron1.adjustWeights();
                hiddenNeuron2.adjustWeights();
            }

            if(epoch < 2000)
                goto Retry;

            Console.ReadLine();
        }

    }
}