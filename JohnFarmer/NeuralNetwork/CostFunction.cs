using System;

namespace JohnFarmer.NeuralNetwork
{
	public struct CostFunction
	{
		public static Func<double, double, double> CrossEntropyLoss = (double x, double y) => -(y * Math.Log(x) + (1 - y) * Math.Log(1 - x));
	}
}
