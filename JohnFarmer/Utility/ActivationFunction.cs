using System;

namespace JohnFarmer.NeuralNetwork
{
	public struct ActivationFunction
	{
		public static Func<double, double> Sigmoid = (double x) => 1 / (1 + Math.Exp(-x));
		public static Func<double, double> SigmoidPrime = delegate (double x) {
			static double Sigmoid(double x) => 1 / (1 + Math.Exp(-x));
			return Sigmoid(x) * (1 - Sigmoid(x));
		};

		public static Func<double, double> ReLU = (double x) => Math.Max(0, x);
		public static Func<double, double> ReLUPrime = (double x) => x < 0 ? 0 : 1;

		public static Func<double, double> SoftPlus = (double x) => Math.Log(1 + Math.Exp(x));
		public static Func<double, double> SoftPlusPrime = (double x) => Sigmoid(x);

		public static Func<double, double> TanH = (double x) => Math.Tanh(x);
		public static Func<double, double> TanHPrime = (double x) => 1 - Math.Pow(Math.Tanh(x), 2);

		public static Func<double, double> ArcTan = (double x) => Math.Atan(x);
		public static Func<double, double> ArcTanPrime = (double x) => 1 / (Math.Pow(x, 2) + 1);
	}
}
