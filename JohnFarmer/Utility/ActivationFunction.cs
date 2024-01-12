using System;

namespace JohnFarmer.NeuralNetwork
{
	public struct ActivationFunction
	{
		public static Func<dynamic, dynamic> Sigmoid = (dynamic x) => 1 / (1 + Math.Exp(-x));
		public static Func<dynamic, dynamic> SigmoidPrime = (dynamic x) => Sigmoid(x) * (1 - Sigmoid(x));

		public static Func<dynamic, dynamic> ReLU = (dynamic x) => Math.Max(0, x);
		public static Func<dynamic, dynamic> ReLUPrime = (dynamic x) => x < 0 ? 0 : 1;

		public static Func<dynamic, dynamic> SoftPlus = (dynamic x) => Math.Log(1 + Math.Exp(x));
		public static Func<dynamic, dynamic> SoftPlusPrime = (dynamic x) => Sigmoid(x);

		public static Func<dynamic, dynamic> Tanh = (dynamic x) => Math.Tanh(x);
		public static Func<dynamic, dynamic> TanhPrime = (dynamic x) => Math.Pow(1 / Math.Cosh(x), 2);

		public static Func<dynamic, dynamic> ArcTan = (dynamic x) => Math.Atan(x);
		public static Func<dynamic, dynamic> ArcTanPrime = (dynamic x) => 1 / (Math.Pow(x, 2) + 1);
	}
}
