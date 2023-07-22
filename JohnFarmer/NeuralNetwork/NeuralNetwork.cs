using System;
using System.Collections.Generic;
using JohnFarmer.Mathematics;

namespace JohnFarmer.NeuralNetwork
{
	public class NeuralNetwork
	{
		public readonly int[] layers;
		public List<Matrix> weights, biases;
		public Func<double, double> activationFunction, activationFunctionDerivative;
		public Func<double, double, double> costFunction;
		public double learningRate, errorMaxRange, accuracy = 0;

		/// <summary>
		/// Initialize the neural network with number of node in each layer, activation function, activation function derivative, cost function and learning rate.
		/// </summary>
		/// <param name="layers">The number of node in each layer.</param>
		/// <param name="activationFunction">A function that decides whether a neuron should be activated or not.</param>
		/// <param name="activationFunctionDerivative">The derivative of the activation function.</param>
		/// <param name="costFunction">A function that used to estimate how badly neural network are performing.</param>
		/// <param name="learningRate">Determine how fast the neural network will adjust it's parameters in each iteration of training.</param>
		/// <param name="errorMaxRange">The maximum difference between the outputs and target outputs can have to be consider correct.</param>
		public NeuralNetwork(int[] layers, Func<double, double> activationFunction, Func<double, double> activationFunctionDerivative, Func<double, double, double> costFunction, double learningRate = .1, double errorMaxRange = .1)
		{
			this.layers = layers;
			this.activationFunction = activationFunction;
			this.activationFunctionDerivative = activationFunctionDerivative;
			this.costFunction = costFunction;
			this.learningRate = learningRate;
			this.errorMaxRange = errorMaxRange;
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
			Matrix outputs = activations[^1];
			Matrix dcdz = outputs - targetOutputs.To1DMatrix();
			for (int l = layers.Length - 1; l > 0; l--)
			{
				dcdz = l == layers.Length - 1 ? dcdz : Matrix.HadamardProduct(weights[l].Transpose() * dcdz, Matrix.Map(weightedSums[l], activationFunctionDerivative));
				weights[l - 1] -= dcdz * activations[l - 1].Transpose() * learningRate;
				biases[l - 1] -= dcdz * learningRate;
			}
			double[] outputsArray = outputs.ToArray();
			int correctPrediction = 0;
			for(int i = 0; i < outputsArray.Length; i++)
				if (Math.Abs(outputsArray[i] - targetOutputs[i]) <= errorMaxRange)
					correctPrediction += 1;
			accuracy = correctPrediction / targetOutputs.Length * 100;
		}
	}
}
