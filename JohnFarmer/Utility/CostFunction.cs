using System;

namespace MathAndAI.NeuralNetwork
{
	public struct CostFunction
	{
		public static Func<double, double, double> CrossEntropyLoss = (double x, double y) => -(y * Math.Log(x) + (1 - y) * Math.Log(1 - x));
		public static Func<double, double, double> SquaredError = (double x, double y) => Math.Pow(y - x, 2);
	}
}
