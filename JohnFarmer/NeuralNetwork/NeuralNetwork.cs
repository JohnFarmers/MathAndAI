using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.HighPerformance;
using JohnFarmer.Mathematics;
using JohnFarmer.Utility;

namespace JohnFarmer.NeuralNetwork.Matrices
{
	public class NeuralNetwork
	{
		public readonly int[] layerNodes;
		public List<Variable> weights, biases;
		public Func<dynamic, dynamic> activationFunction, activationFunctionDerivative;
		public Func<dynamic, dynamic, dynamic> lossFunction;
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
		public NeuralNetwork(int[] layerNodes, Func<dynamic, dynamic> activationFunction, Func<dynamic, dynamic> activationFunctionDerivative, Func<dynamic, dynamic, dynamic> lossFunction, double learningRate = .1, double errorMaxRange = .1)
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
			this.weights = new List<Variable>();
			this.biases = new List<Variable>();
			for (int i = 0; i < layerNodes.Length - 1; i++)
			{
				this.weights.Add(new Variable(weights == null ? new Matrix(layerNodes[i + 1], layerNodes[i]).Randomize() : weights[i], true));
				this.biases.Add(new Variable(biases == null ? new Matrix(layerNodes[i + 1], 1).Randomize() : biases[i], true));
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
				prediction = Matrix.MatMul(weights[i].value, (i == 0 ? matrixInputs : prediction)) + biases[i].value;
				prediction.Map(activationFunction);
			}
			return prediction.ToArray();
		}

		/// <summary>
		/// Perform a forward propagatiom and train the neural network by performing a backpropagation.
		/// </summary>
		public virtual void Train(double[] inputs, double[] targetOutputs)
		{
			Matrix matrixInputs = inputs.To1DMatrix();
			Operation activation = null;
			for (int i = 0; i < layerNodes.Length - 1; i++)
			{
				Operation mul = i == 0 ? Operation.MatMul(weights[i], matrixInputs) : Operation.MatMul(weights[i], activation);
				Operation add = mul + biases[i];
				activation = Operation.Sigmoid(add);
			}
			Operation loss = Operation.SquaredError(activation, targetOutputs.To1DMatrix());
			loss.Backward();
			Span<Variable> span = weights.AsSpan();
			for (int i = 0; i < span.Length; i++)
				span[i].Optimize(learningRate);
			span = biases.AsSpan();
			for (int i = 0; i < span.Length; i++)
				span[i].Optimize(learningRate);
			/*for (int i = 0; i < outputsArray.Length; i++)
				if (Math.Abs(outputsArray[i] - targetOutputs[i]) <= errorMaxRange)
					correctPrediction += 1;
			accuracy = correctPrediction / targetOutputs.Length * 100;*/
		}
	}
}
