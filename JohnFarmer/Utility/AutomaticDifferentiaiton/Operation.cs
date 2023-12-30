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
		public readonly Type aType;
		public readonly Type bType;

		private Operation(dynamic a, dynamic b, dynamic result, dynamic dyda, dynamic dydb)
		{
			this.a = a;
			this.b = b;
			this.result = result;
			this.dyda = dyda;
			this.dydb = dydb;
			this.aType = a == null ? null : a.GetType();
			this.bType = b == null ? null : b.GetType();
		}

		public static dynamic operator +(Operation a, Operation b) => a.result + b.result;

		public static dynamic operator +(Operation a, dynamic b) => a.result + b;

		public static dynamic operator -(Operation a, Operation b) => a.result - b.result;

		public static dynamic operator -(Operation a, dynamic b) => a.result - b;

		public static dynamic operator *(Operation a, Operation b) => a.result * b.result;

		public static dynamic operator *(Operation a, dynamic b) => a.result * b;

		public static dynamic operator /(Operation a, Operation b) => a.result / b.result;

		public static dynamic operator /(Operation a, dynamic b) => a.result / b;

		public static Operation Add(dynamic a, dynamic b) => new Operation(a, b, a + b, 1d, 1d);

		public static Operation Subtract(dynamic a, dynamic b) => new Operation(a, b, a - b, 1d, -1d);

		public static Operation Multiply(dynamic a, dynamic b) => new Operation(a, b, a * b, b, a);

		public static Operation Divide(dynamic a, dynamic b) => new Operation(a, b, a / b, 1d / b, -(a / (b ^ 2d)));

		public static Operation Power(dynamic a, dynamic b) => new Operation(a, b, a ^ b , b * (a ^ (b - 1d)), (a ^ b) * Math.Log(a));
		
		public static Operation Root(dynamic a, dynamic b) => new Operation(a, b, a ^ (1d / b), (a ^ ((1d / b) - 1d)) / b, -(((a ^ (1d / b)) * Math.Log(a)) / (b ^ 2d)));

		public static Operation Sin(dynamic a) => new Operation(a, null, Math.Sin(a), Math.Cos(a), null);

		public static Operation Cos(dynamic a) => new Operation(a, null, Math.Cos(a), -Math.Sin(a), null);

		public static Operation Tan(dynamic a) => new Operation(a, null, Math.Tan(a), (1d / Math.Cos(a)) ^ 2d, null);

		public static Operation Log(dynamic a) => new Operation(a, null, Math.Log(a), 1d / a, null);

		public static Operation Log10(dynamic a) => new Operation(a, null, Math.Log10(a), 1d / (a * Math.Log(10)), null);

		public static Operation Exp(dynamic a) => new Operation(a, null, Math.Exp(a), Math.Exp(a), null);

		public static Operation Sigmoid(dynamic a) => new Operation(a, null, ActivationFunction.Sigmoid(a), ActivationFunction.SigmoidPrime(a), null);
		
		public static Operation ReLU(dynamic a) => new Operation(a, null, ActivationFunction.ReLU(a), ActivationFunction.ReLUPrime(a), null);

		public static Operation SoftPlus(dynamic a) => new Operation(a, null, ActivationFunction.SoftPlus(a), ActivationFunction.SoftPlusPrime(a), null);

		public static Operation Tanh(dynamic a) => new Operation(a, null, ActivationFunction.Tanh(a), ActivationFunction.TanhPrime(a), null);

		public static Operation ArcTan(dynamic a) => new Operation(a, null, ActivationFunction.ArcTan(a), ActivationFunction.ArcTanPrime(a), null);

		public static Operation CrossEntropyLoss(dynamic a, dynamic b) => new Operation(a, b, LossFunction.CrossEntropyLoss(a, b), -(b / a), -Math.Log(a));

		public static Operation SquaredError(dynamic a, dynamic b) => new Operation(a, b, LossFunction.SquaredError(a, b), -2d * (b - a), 2d * (b - a));
		
		public override string ToString() => $"a = {a}\nb = {b}\nresult = {result}\ndyda = {dyda}\ndydb = {dydb}";

		public static implicit operator int(Operation operation) => Convert.ChangeType(operation.result, typeof(int));

		public static implicit operator float(Operation operation) => Convert.ChangeType(operation.result, typeof(float));

		public static implicit operator double(Operation operation) => Convert.ChangeType(operation.result, typeof(double));

		public static implicit operator long(Operation operation) => Convert.ChangeType(operation.result, typeof(long));
	}
}
