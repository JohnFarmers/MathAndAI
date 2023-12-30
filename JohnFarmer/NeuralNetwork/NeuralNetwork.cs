using System;
using System.Collections.Generic;
using System.Linq;
using JohnFarmer.Mathematics;

namespace JohnFarmer.NeuralNetwork.Matrices
{
	public class NeuralNetwork
	{
		public readonly int[] layerNodes;
		public List<Matrix> weights, biases;
		public Func<double, double> activationFunction, activationFunctionDerivative;
		public Func<double, double, double> lossFunction;
		public double learningRate, errorMaxRange, accuracy = 0;

		/// <summary>
		/// Initialize the neural network variables.
		/// </summary>
		/// <param name="layerNodes">The number of node in each layer.</param>
		/// <param name="activationFunction">A function that decides whether a neuron should be activated or not.</param>
		/// <param name="activationFunctionDerivative">The derivative of the activation function.</param>
		/// <param name="lossFunction">A function that used to estimate how badly neural network are performing.</param>
		/// <param name="learningRate">Determine how fast the neural network will adjust it's parameters in each iteration of training.</param>
		/// <param name="errorMaxRange">The maximum difference between the outputs and target outputs can have to be consider correct.</param>
		public NeuralNetwork(int[] layerNodes, Func<double, double> activationFunction, Func<double, double> activationFunctionDerivative, Func<double, double, double> lossFunction, double learningRate = .1, double errorMaxRange = .1)
		{
			if (layerNodes.Length < 2)
				throw new Exception("Neural Network must have atleast 2 layers");
			this.layerNodes = layerNodes;
			this.activationFunction = activationFunction;
			this.activationFunctionDerivative = activationFunctionDerivative;
			this.lossFunction = lossFunction;
			this.learningRate = learningRate;
			this.errorMaxRange = errorMaxRange;
			Initialize();
		}

		/// <summary>
		/// Initialize or reset the neural network with specified number of node in each layer.
		/// </summary>
		/// <param name="weights">A parameter for when you want to specify the weights by yourself.</param>
		/// <param name="biases">A parameter for when you want to specify the biases by yourself.</param>
		public virtual void Initialize(Matrix[] weights = null, Matrix[] biases = null)
		{
			this.weights = new List<Matrix>();
			this.biases = new List<Matrix>();
			for (int i = 0; i < layerNodes.Length - 1; i++)
			{
				this.weights.Add(weights == null ? new Matrix(layerNodes[i + 1], layerNodes[i]).Randomize() : weights[i]);
				this.biases.Add(biases == null ? new Matrix(layerNodes[i + 1], 1).Randomize() : biases[i]);
			}
		}

		/// <summary>
		/// Perform a forward propagation and return the predicted value.
		/// </summary>
		public virtual double[] Forward(double[] inputs)
		{
			Matrix prediction = new Matrix();
			Matrix matrixInputs = inputs.To1DMatrix();
			for (int i = 0; i < layerNodes.Length - 1; i++)
			{
				prediction = (weights[i] * (i == 0 ? matrixInputs : prediction)) + biases[i];
				prediction.Map(activationFunction);
			}
			return prediction.ToArray();
		}

		/// <summary>
		/// Perform a forward propagatiom and train the neural network by performing a backpropagation.
		/// </summary>
		public virtual void Train(double[] inputs, double[] targetOutputs)
		{
			if (targetOutputs.Length != layerNodes[^1])
				throw new Exception("The number of target outputs must match the number of the neural network outputs.");
			List<Matrix> activations = new List<Matrix>(), weightedSums = new List<Matrix>();
			Matrix matrixInputs = inputs.To1DMatrix();
			activations.Add(matrixInputs);
			weightedSums.Add(matrixInputs);
			for (int i = 0; i < layerNodes.Length - 1; i++)
			{
				Matrix weightedSum = (weights[i] * (i == 0 ? matrixInputs : activations[i])) + biases[i];
				weightedSums.Add(weightedSum);
				activations.Add(Matrix.Map(weightedSum, activationFunction));
			}
			Matrix outputs = activations[^1];
			Matrix dcdz = outputs - targetOutputs.To1DMatrix();
			for (int l = layerNodes.Length - 1; l > 0; l--)
			{
				dcdz = l == layerNodes.Length - 1 ? dcdz : Matrix.HadamardProduct(weights[l].Transpose() * dcdz, Matrix.Map(weightedSums[l], activationFunctionDerivative));
				weights[l - 1] -= dcdz * activations[l - 1].Transpose() * learningRate;
				biases[l - 1] -= dcdz * learningRate;
			}
			double[] outputsArray = Forward(inputs).ToArray();
			int correctPrediction = 0;
			for(int i = 0; i < outputsArray.Length; i++)
				if (Math.Abs(outputsArray[i] - targetOutputs[i]) <= errorMaxRange)
					correctPrediction += 1;
			accuracy = correctPrediction / targetOutputs.Length * 100;
		}
	}
}
