using JohnFarmer.NeuralNetwork;
using System;

namespace JohnFarmer.Utility
{
	public class Operation
	{
		public readonly dynamic a;
		public readonly dynamic b;
		public readonly dynamic result;
		public readonly dynamic dyda;
		public readonly dynamic dydb;

		private Operation(dynamic a, dynamic b, dynamic result, dynamic dyda, dynamic dydb)
		{
			this.a = a;
			this.b = b;
			this.result = result;
			this.dyda = dyda;
			this.dydb = dydb;
		}

		public static Operation Add(dynamic a, dynamic b) => new Operation(a, b, a + b, 1, 1);

		public static Operation Subtract(dynamic a, dynamic b) => new Operation(a, b, a - b, 1, 1);

		public static Operation Multiply(dynamic a, dynamic b) => new Operation(a, b, a * b, b, a);

		public static Operation Divide(dynamic a, dynamic b) => new Operation(a, b, a / b, 1 / b, -(a / (b ^ 2)));

		public static Operation Power(dynamic a, dynamic b) => new Operation(a, b, a ^ b , b * (a ^ (b - 1)), (a ^ b) * Math.Log(a));
		
		public static Operation Root(dynamic a, dynamic b) => new Operation(a, b, a ^ (1 / b), (a ^ ((1 / b) - 1)) / b, -(((a ^ (1 / b)) * Math.Log(a)) / (b ^ 2)));

		public static Operation Sin(dynamic a) => new Operation(a, null, Math.Sin(a), Math.Cos(a), null);

		public static Operation Cos(dynamic a) => new Operation(a, null, Math.Cos(a), -Math.Sin(a), null);

		public static Operation Tan(dynamic a) => new Operation(a, null, Math.Tan(a), (1 / Math.Cos(a)) ^ 2, null);

		public static Operation Log(dynamic a) => new Operation(a, null, Math.Log(a), 1 / a, null);

		public static Operation Log10(dynamic a) => new Operation(a, null, Math.Log10(a), 1 / (a * Math.Log(10)), null);

		public static Operation Exp(dynamic a) => new Operation(a, null, Math.Exp(a), Math.Exp(a), null);

		public static Operation Sigmoid(dynamic a) => new Operation(a, null, ActivationFunction.Sigmoid(a), ActivationFunction.SigmoidPrime(a), null);
		
		public static Operation ReLU(dynamic a) => new Operation(a, null, ActivationFunction.ReLU(a), ActivationFunction.ReLUPrime(a), null);

		public static Operation SoftPlus(dynamic a) => new Operation(a, null, ActivationFunction.SoftPlus(a), ActivationFunction.SoftPlusPrime(a), null);

		public static Operation Tanh(dynamic a) => new Operation(a, null, ActivationFunction.Tanh(a), ActivationFunction.TanhPrime(a), null);

		public static Operation ArcTan(dynamic a) => new Operation(a, null, ActivationFunction.ArcTan(a), ActivationFunction.ArcTanPrime(a), null);
	}
}
