using System;

namespace JohnFarmer.NeuralNetwork
{
	public struct ActivationFunction
	{
		public static Func<double, double> Sigmoid = (double x) => 1 / (1 + System.Math.Exp(-x));
		public static Func<double, double> SigmoidPrime = delegate (double x) {
			static double Sigmoid(double x) => 1 / (1 + System.Math.Exp(-x));
			return Sigmoid(x) * (1 - Sigmoid(x));
		};
	}
}
