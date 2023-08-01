using System;
using System.Collections.Generic;
using JohnFarmer.Mathematics;

namespace JohnFarmer.NeuralNetwork
{
	public class ConvolutionalNeuralNetwork : NeuralNetwork
	{
		private Matrix kernel;

		public ConvolutionalNeuralNetwork(int[] layers, Func<double, double> activationFunction, Func<double, double> activationFunctionDerivative, Func<double, double, double> costFunction, Matrix kernel, double learningRate = 0.1, double errorMaxRange = 0.1) : base(layers, activationFunction, activationFunctionDerivative, costFunction, learningRate, errorMaxRange)
		{
			this.kernel = kernel;
		}

		public override double[] Predict(double[] inputs)
		{
			return base.Predict(inputs);
		}

		public override void Train(double[] inputs, double[] targetOutputs)
		{
			base.Train(inputs, targetOutputs);
		}
	}
}
