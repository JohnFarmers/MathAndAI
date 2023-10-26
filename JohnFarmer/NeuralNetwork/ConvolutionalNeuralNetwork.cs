using System;
using System.Linq;
using MathAndAI.Mathematics;

namespace MathAndAI.NeuralNetwork
{
	public class ConvolutionalNeuralNetwork : NeuralNetwork
	{
		public ConvolutionalNeuralNetwork(int[] layers, Func<double, double> activationFunction, Func<double, double> activationFunctionDerivative, Func<double, double, double> costFunction, double learningRate = 0.1, double errorMaxRange = 0.1) : base(layers, activationFunction, activationFunctionDerivative, costFunction, learningRate, errorMaxRange) { }

		public double[] Predict(double[,] inputs, double[,] kernel)
		{
			Matrix featureMap = Convolve(inputs.ToMatrix(), kernel.ToMatrix());
			featureMap.Map(ActivationFunction.ReLU);
			Matrix maxPool = MaxPool(featureMap, Convert.ToInt32(featureMap.rows / 2), Convert.ToInt32(featureMap.columns / 2));
			return base.Predict(maxPool.ToArray());
		}

		private Matrix Convolve(Matrix inputs, Matrix kernel)
		{
			int kRows = kernel.rows;
			int kColumns = kernel.columns;
			Matrix result = new Matrix(inputs.rows - kRows + 1, inputs.columns - kColumns + 1);
			for (int row = 0; row < result.rows; row++)
				for (int column = 0; column < result.columns; column++)
					result[row, column] = Matrix.HadamardProduct(inputs[row..(row + kRows), column..(column + kColumns)], kernel).GetSum();
			return result;
		}

		private Matrix MaxPool(Matrix inputs, int rows, int columns)
		{
			rows += 1;
			columns += 1;
			Matrix result = new Matrix(inputs.rows - rows + 1, inputs.columns - columns + 1);
			for (int row = 0; row < result.rows; row++)
				for (int column = 0; column < result.columns; column++)
					result[row, column] = inputs[row..(row + rows), column..(column + columns)].ToArray().Max();
			return result;
		}
	}
}
