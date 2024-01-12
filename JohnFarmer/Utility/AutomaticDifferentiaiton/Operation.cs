using System;
using JohnFarmer.Mathematics;
using JohnFarmer.NeuralNetwork;

namespace JohnFarmer.Utility
{
	public partial class Operation
	{
		public readonly dynamic a;
		public readonly dynamic b;
		public readonly dynamic result;
		public readonly dynamic dyda;
		public readonly dynamic dydb;
		public readonly Type aType;
		public readonly Type bType;
		public readonly Type resultType;
		public readonly OperationType type;

		public Operation(dynamic a, dynamic b, dynamic result, dynamic dyda, dynamic dydb, OperationType type)
		{
			this.a = a;
			this.b = b;
			this.result = result;
			this.dyda = dyda;
			this.dydb = dydb;
			this.aType = a?.GetType();
			this.bType = b?.GetType();
			this.resultType = result?.GetType();
			this.type = type;
		}

		public static Operation operator +(Operation a, Operation b) => new Operation(a, b, a.result + b.result, 1d, 1d, OperationType.Add);

		public static Operation operator +(Operation a, dynamic b) => new Operation(a, b, a.result + b, 1d, 1d, OperationType.Add);

		public static Operation operator -(Operation a, Operation b) => new Operation(a, b, a.result - b.result, 1d, -1d, OperationType.Subtract);

		public static Operation operator -(Operation a, dynamic b) => new Operation(a, b, a.result - b, 1d, -1d, OperationType.Subtract);

		public static Operation operator *(Operation a, Operation b) => new Operation(a, b, a.result * b.result, b.result, a.result, OperationType.Multiply);

		public static Operation operator *(Operation a, dynamic b) => new Operation(a, b, a.result * b, b, a.result, OperationType.Multiply);

		public static Operation operator /(Operation a, Operation b) => new Operation(a, b, a.result / b.result, 1d / b.result, -(a.result / (b.result ^ 2d)), OperationType.Divide);

		public static Operation operator /(Operation a, dynamic b) => new Operation(a, b, a.result / b, 1d / b, -(a.result / (b ^ 2d)), OperationType.Divide);

		public static Operation operator ^(Operation a, Operation b) => new Operation(a, b, a.result ^ b.result, b.result * (a.result ^ (b.result - 1d)), (a.result ^ b.result) * Math.Log(a.result), OperationType.Power);

		public static Operation operator ^(Operation a, dynamic b) => new Operation(a, b, a.result ^ b, b * (a.result ^ (b - 1d)), (a.result ^ b) * Math.Log(a.result), OperationType.Power);

		public static Operation Add(dynamic a, dynamic b) => new Operation(a, b, a + b, 1d, 1d, OperationType.Add);

		public static Operation Subtract(dynamic a, dynamic b) => new Operation(a, b, a - b, 1d, -1d, OperationType.Subtract);

		public static Operation Multiply(dynamic a, dynamic b) => new Operation(a, b, a * b, b, a, OperationType.Multiply);

		public static Operation Divide(dynamic a, dynamic b) => new Operation(a, b, a / b, 1d / b, -(a / (b ^ 2d)), OperationType.Divide);

		public static Operation Power(dynamic a, dynamic b) => new Operation(a, b, a ^ b, b * (a ^ (b - 1d)), (a ^ b) * Math.Log(a), OperationType.Power);
		
		public static Operation Root(dynamic a, dynamic b) => new Operation(a, b, a ^ (1d / b), (a ^ ((1d / b) - 1d)) / b, -(((a ^ (1d / b)) * Math.Log(a)) / (b ^ 2d)), OperationType.Root);

		public static Operation Sin(dynamic a) => new Operation(a, null, Math.Sin(a), Math.Cos(a), null, OperationType.Sin);

		public static Operation Cos(dynamic a) => new Operation(a, null, Math.Cos(a), -Math.Sin(a), null, OperationType.Cos);

		public static Operation Tan(dynamic a) => new Operation(a, null, Math.Tan(a), (1d / Math.Cos(a)) ^ 2d, null, OperationType.Tan);

		public static Operation Log(dynamic a) => new Operation(a, null, Math.Log(a), 1d / a, null, OperationType.Log);

		public static Operation Log10(dynamic a) => new Operation(a, null, Math.Log10(a), 1d / (a * Math.Log(10)), null, OperationType.Log10);

		public static Operation Exp(dynamic a) => new Operation(a, null, Math.Exp(a), Math.Exp(a), null, OperationType.Exp);

		public static Operation Sigmoid(dynamic a) => new Operation(a, null, ActivationFunction.Sigmoid(a), ActivationFunction.SigmoidPrime(a), null, OperationType.Sigmoid);
		
		public static Operation ReLU(dynamic a) => new Operation(a, null, ActivationFunction.ReLU(a), ActivationFunction.ReLUPrime(a), null, OperationType.ReLU);

		public static Operation SoftPlus(dynamic a) => new Operation(a, null, ActivationFunction.SoftPlus(a), ActivationFunction.SoftPlusPrime(a), null, OperationType.SoftPlus);

		public static Operation Tanh(dynamic a) => new Operation(a, null, ActivationFunction.Tanh(a), ActivationFunction.TanhPrime(a), null, OperationType.Tanh);

		public static Operation ArcTan(dynamic a) => new Operation(a, null, ActivationFunction.ArcTan(a), ActivationFunction.ArcTanPrime(a), null, OperationType.ArcTan);

		public static Operation CrossEntropyLoss(dynamic a, dynamic b) => new Operation(a, b, LossFunction.CrossEntropyLoss(a, b), -(b / a), -Math.Log(a), OperationType.CrossEntropyLoss);

		public static Operation SquaredError(dynamic a, dynamic b) => new Operation(a, b, LossFunction.SquaredError(a, b), -2d * (b - a), 2d * (b - a), OperationType.SquaredError);

		public static Operation Add(Matrix a, Matrix b) => new Operation(a, b, a + b, new Matrix(a.rows, a.columns, 1d), new Matrix(b.rows, b.columns, 1d), OperationType.Add);

		public static Operation Subtract(Matrix a, Matrix b) => new Operation(a, b, a - b, new Matrix(a.rows, a.columns, 1d), new Matrix(b.rows, b.columns, -1d), OperationType.Subtract);

		public static Operation Multiply(Matrix a, Matrix b) => new Operation(a, b, a * b, b, a, OperationType.Multiply);

		/*public static Operation Divide(Matrix a, Matrix b) => new Operation(a, b, a / b, 1d / b, -(a / (b ^ 2d)), OperationType.Divide);

		public static Operation Power(dynamic a, dynamic b) => new Operation(a, b, a ^ b, b * (a ^ (b - 1d)), (a ^ b) * Math.Log(a), OperationType.Power);

		public static Operation Root(dynamic a, dynamic b) => new Operation(a, b, a ^ (1d / b), (a ^ ((1d / b) - 1d)) / b, -(((a ^ (1d / b)) * Math.Log(a)) / (b ^ 2d)), OperationType.Root);

		public static Operation Sin(dynamic a) => new Operation(a, null, Math.Sin(a), Math.Cos(a), null, OperationType.Sin);

		public static Operation Cos(dynamic a) => new Operation(a, null, Math.Cos(a), -Math.Sin(a), null, OperationType.Cos);

		public static Operation Tan(dynamic a) => new Operation(a, null, Math.Tan(a), (1d / Math.Cos(a)) ^ 2d, null, OperationType.Tan);

		public static Operation Log(dynamic a) => new Operation(a, null, Math.Log(a), 1d / a, null, OperationType.Log);

		public static Operation Log10(dynamic a) => new Operation(a, null, Math.Log10(a), 1d / (a * Math.Log(10)), null, OperationType.Log10);

		public static Operation Exp(dynamic a) => new Operation(a, null, Math.Exp(a), Math.Exp(a), null, OperationType.Exp);*/

		public static Operation Sigmoid(Matrix a) => new Operation(a, null, a.Map(ActivationFunction.Sigmoid), a.Map(ActivationFunction.SigmoidPrime), null, OperationType.Sigmoid);

		public static Operation ReLU(Matrix a) => new Operation(a, null, a.Map(ActivationFunction.ReLU), a.Map(ActivationFunction.ReLUPrime), null, OperationType.ReLU);

		public static Operation SoftPlus(Matrix a) => new Operation(a, null, a.Map(ActivationFunction.SoftPlus), a.Map(ActivationFunction.SoftPlusPrime), null, OperationType.SoftPlus);

		public static Operation Tanh(Matrix a) => new Operation(a, null, a.Map(ActivationFunction.Tanh), a.Map(ActivationFunction.TanhPrime), null, OperationType.Tanh);

		public static Operation ArcTan(Matrix a) => new Operation(a, null, a.Map(ActivationFunction.ArcTan), a.Map(ActivationFunction.ArcTanPrime), null, OperationType.ArcTan);

		public static Operation CrossEntropyLoss(Matrix a, Matrix b) => new Operation(a, b, LossFunction.M_CrossEntropyLoss(a, b), (b / a) * -1d, a.Map((dynamic x) => Math.Log(x)) * -1d, OperationType.CrossEntropyLoss);

		public static Operation SquaredError(Matrix a, Matrix b) => new Operation(a, b, LossFunction.M_SquaredError(a, b), (b - a) * -2d, (b - a) * 2d, OperationType.SquaredError);

		public override string ToString() => $"a = {a},\nb = {b},\nresult = {result},\ndyda = {dyda},\ndydb = {dydb}";

		public static implicit operator int(Operation operation) => (int)operation.result;

		public static implicit operator float(Operation operation) => (float)operation.result;

		public static implicit operator double(Operation operation) => (double)operation.result;

		public static implicit operator long(Operation operation) => (long)operation.result;
	}
}
