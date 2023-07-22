using System;
using System.Collections.Generic;
using JohnFarmer.Mathematics;

namespace JohnFarmer.NeuralNetwork
{
    public class NeuralNetwork
    {
        public readonly int[] layers;
        public List<Matrix> weights, biases;
        public Func<double, double> activationFunction = ActivationFunction.Sigmoid, activationFunctionDerivative = ActivationFunction.SigmoidPrime;
        public Func<double, double, double> costFunction = CostFunction.CrossEntropyLoss;
        public double learningRate = .1;

        public NeuralNetwork(params int[] layers)
        {
            this.layers = layers;
            weights = new List<Matrix>();
            biases = new List<Matrix>();
			for (int i = 0; i < layers.Length - 1; i++)
            {
                weights.Add(new Matrix(layers[i + 1], layers[i]).Randomize());
                biases.Add(new Matrix(layers[i + 1], 1).Randomize());
            }
        }

		/// <summary>
		/// Perform a forward propagation and return the predicted value.
		/// </summary>
		public double[] Predict(double[] inputs)
        {
            Matrix prediction = new Matrix();
            Matrix matrixInputs = inputs.To1DMatrix();
            for (int i = 0; i < layers.Length - 1; i++)
            {
                prediction = (weights[i] * (i == 0 ? matrixInputs : prediction)) + biases[i];
				prediction.Map(activationFunction);
            }
            return prediction.ToArray();
        }

		/// <summary>
		/// Perform a forward propagatiom and train the neural network by performing a backpropagation.
		/// </summary>
		public void Train(double[] inputs, double[] targetOutputs)
        {
            if (targetOutputs.Length != layers[^1])
                throw new Exception("The number of target outputs must match the number of the neural network outputs.");
			List<Matrix> activations = new List<Matrix>(), weightedSums = new List<Matrix>();
			Matrix matrixInputs = inputs.To1DMatrix();
            activations.Add(matrixInputs);
            weightedSums.Add(matrixInputs);
			for (int i = 0; i < layers.Length - 1; i++)
            {
                Matrix weightedSum = (weights[i] * (i == 0 ? matrixInputs : activations[i])) + biases[i];
                weightedSums.Add(weightedSum);
				activations.Add(Matrix.Map(weightedSum, activationFunction));
            }
            Matrix dcdz = activations[^1] - targetOutputs.To1DMatrix();
			for (int l = layers.Length - 1; l > 0; l--)
			{
                dcdz = l == layers.Length - 1 ? dcdz : Matrix.HadamardProduct(weights[l].Transpose() * dcdz, Matrix.Map(weightedSums[l], activationFunctionDerivative));
                weights[l - 1] -= (dcdz * activations[l - 1].Transpose()) * learningRate;
                biases[l - 1] -= dcdz * learningRate;
            }
        }
    }
}
