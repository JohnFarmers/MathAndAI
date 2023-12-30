using System;

namespace JohnFarmer.NeuralNetwork
{
	public struct LossFunction
	{
		public static Func<double, double, double> CrossEntropyLoss = (double x, double y) => -(y * Math.Log(x));
		public static Func<double, double, double> SquaredError = (double x, double y) => Math.Pow(y - x, 2);
	}
}
