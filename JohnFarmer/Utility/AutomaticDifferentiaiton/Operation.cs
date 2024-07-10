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

		public static Operation operator +(Operation a, Operation b)
		{
			bool aIsMatrix = a.resultType == typeof(Matrix);
			bool bIsMatrix = b.resultType == typeof(Matrix);

			if (aIsMatrix && bIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.result + b.result, new Matrix(row, col, 1d), new Matrix(row, col, 1d), OperationType.Add);
			}
			else if (aIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.result + b.result, new Matrix(row, col, 1d), row * col, OperationType.Add);
			}
			else if (bIsMatrix)
			{
				Matrix B = (Matrix)b.result;
				int row = B.rows;
				int col = B.columns;
				return new Operation(a, b, a.result + b.result, row * col, new Matrix(row, col, 1d), OperationType.Add);
			}

			return new Operation(a, b, a.result + b.result, 1d, 1d, OperationType.Add);
		}

		public static Operation operator +(Operation a, Variable b)
		{
			bool aIsMatrix = a.resultType == typeof(Matrix);
			bool bIsMatrix = b.type == typeof(Matrix);

			if (aIsMatrix && bIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.result + b.value, new Matrix(row, col, 1d), new Matrix(row, col, 1d), OperationType.Add);
			}
			else if (aIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.result + b.value, new Matrix(row, col, 1d), row * col, OperationType.Add);
			}
			else if (bIsMatrix)
			{
				Matrix B = (Matrix)b.value;
				int row = B.rows;
				int col = B.columns;
				return new Operation(a, b, a.result + b.value, row * col, new Matrix(row, col, 1d), OperationType.Add);
			}

			return new Operation(a, b, a.result + b.value, 1d, 1d, OperationType.Add);
		}

		public static Operation operator +(Operation a, dynamic b)
		{
			bool aIsMatrix = a.resultType == typeof(Matrix);
			bool bIsMatrix = b.GetType() == typeof(Matrix);

			if (aIsMatrix && bIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.result + b, new Matrix(row, col, 1d), new Matrix(row, col, 1d), OperationType.Add);
			}
			else if (aIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.result + b, new Matrix(row, col, 1d), row * col, OperationType.Add);
			}
			else if (bIsMatrix)
			{
				Matrix B = (Matrix)b;
				int row = B.rows;
				int col = B.columns;
				return new Operation(a, b, a.result + b, row * col, new Matrix(row, col, 1d), OperationType.Add);
			}

			return new Operation(a, b, a.result + b, 1d, 1d, OperationType.Add);
		}

		public static Operation operator -(Operation a, Operation b)
		{
			bool aIsMatrix = a.resultType == typeof(Matrix);
			bool bIsMatrix = b.resultType == typeof(Matrix);

			if (aIsMatrix && bIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.result - b.result, new Matrix(row, col, 1d), new Matrix(row, col, -1d), OperationType.Subtract);
			}
			else if (aIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.result - b.result, new Matrix(row, col, 1d), -row * col, OperationType.Subtract);
			}
			else if (bIsMatrix)
			{
				Matrix B = (Matrix)b.result;
				int row = B.rows;
				int col = B.columns;
				return new Operation(a, b, a.result - b.result, row * col, new Matrix(row, col, -1d), OperationType.Subtract);
			}

			return new Operation(a, b, a.result - b.result, 1d, -1d, OperationType.Subtract);
		}

		public static Operation operator -(Operation a, Variable b)
		{
			bool aIsMatrix = a.resultType == typeof(Matrix);
			bool bIsMatrix = b.type == typeof(Matrix);

			if (aIsMatrix && bIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.result - b.value, new Matrix(row, col, 1d), new Matrix(row, col, -1d), OperationType.Subtract);
			}
			else if (aIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.result - b.value, new Matrix(row, col, 1d), -row * col, OperationType.Subtract);
			}
			else if (bIsMatrix)
			{
				Matrix B = (Matrix)b.value;
				int row = B.rows;
				int col = B.columns;
				return new Operation(a, b, a.result - b.value, row * col, new Matrix(row, col, -1d), OperationType.Subtract);
			}

			return new Operation(a, b, a.result - b.value, 1d, -1d, OperationType.Subtract);
		}

		public static Operation operator -(Operation a, dynamic b)
		{
			bool aIsMatrix = a.resultType == typeof(Matrix);
			bool bIsMatrix = b.GetType() == typeof(Matrix);

			if (aIsMatrix && bIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.result - b, new Matrix(row, col, 1d), new Matrix(row, col, -1d), OperationType.Subtract);
			}
			else if (aIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.result - b, new Matrix(row, col, 1d), -row * col, OperationType.Subtract);
			}
			else if (bIsMatrix)
			{
				Matrix B = (Matrix)b;
				int row = B.rows;
				int col = B.columns;
				return new Operation(a, b, a.result - b, row * col, new Matrix(row, col, -1d), OperationType.Subtract);
			}

			return new Operation(a, b, a.result - b, 1d, -1d, OperationType.Subtract);
		}

		public static Operation operator *(Operation a, Operation b)
		{
			bool aIsMatrix = a.resultType == typeof(Matrix);
			bool bIsMatrix = b.resultType == typeof(Matrix);

			if (aIsMatrix && bIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				Matrix B = (Matrix)b.result;
				return new Operation(a, b, a.result * b.result, B, A, OperationType.HadamardProduct);
			}
			else if (aIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.result * b.result, new Matrix(row, col, b.result), A.Sum(), OperationType.Multiply);
			}
			else if (bIsMatrix)
			{
				Matrix B = (Matrix)b.result;
				int row = B.rows;
				int col = B.columns;
				return new Operation(a, b, a.result * b.result, B.Sum(), new Matrix(row, col, a.result), OperationType.Multiply);
			}

			return new Operation(a, b, a.result * b.result, b.result, a.result, OperationType.Multiply);
		}

		public static Operation operator *(Operation a, Variable b)
		{
			bool aIsMatrix = a.resultType == typeof(Matrix);
			bool bIsMatrix = b.GetType() == typeof(Matrix);

			if (aIsMatrix && bIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				Matrix B = (Matrix)b.value;
				return new Operation(a, b, a.result * b.value, B, A, OperationType.HadamardProduct);
			}
			else if (aIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.result * b.value, new Matrix(row, col, b.value), A.Sum(), OperationType.Multiply);
			}
			else if (bIsMatrix)
			{
				Matrix B = (Matrix)b.value;
				int row = B.rows;
				int col = B.columns;
				return new Operation(a, b, a.result * b.value, B.Sum(), new Matrix(row, col, a.result), OperationType.Multiply);
			}

			return new Operation(a, b, a.result * b.value, b.value, a.result, OperationType.Multiply);
		}

		public static Operation operator *(Operation a, dynamic b)
		{
			bool aIsMatrix = a.resultType == typeof(Matrix);
			bool bIsMatrix = b.GetType() == typeof(Matrix);

			if (aIsMatrix && bIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				Matrix B = (Matrix)b;
				return new Operation(a, b, a.result * b, B, A, OperationType.HadamardProduct);
			}
			else if (aIsMatrix)
			{
				Matrix A = (Matrix)a.result;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.result * b, new Matrix(row, col, b), A.Sum(), OperationType.Multiply);
			}
			else if (bIsMatrix)
			{
				Matrix B = (Matrix)b;
				int row = B.rows;
				int col = B.columns;
				return new Operation(a, b, a.result * b, B.Sum(), new Matrix(row, col, a.result), OperationType.Multiply);
			}

			return new Operation(a, b, a.result * b, b, a.result, OperationType.Multiply);
		}

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

		public static Operation Sigmoid(Operation a)
		{
			if(a.resultType == typeof(Matrix))
			{
				Matrix A = (Matrix)a.result;
				return new Operation(a, null, Matrix.Map(A, ActivationFunction.Sigmoid), Matrix.Map(A, ActivationFunction.SigmoidPrime), null, OperationType.Sigmoid);
			}
			return new Operation(a, null, ActivationFunction.Sigmoid(a), ActivationFunction.SigmoidPrime(a), null, OperationType.Sigmoid);
		}
		
		public static Operation Sigmoid(Variable a)
		{
			if(a.type == typeof(Matrix))
			{
				Matrix A = (Matrix)a.value;
				return new Operation(a, null, Matrix.Map(A, ActivationFunction.Sigmoid), Matrix.Map(A, ActivationFunction.SigmoidPrime), null, OperationType.Sigmoid);
			}
			return new Operation(a, null, ActivationFunction.Sigmoid(a), ActivationFunction.SigmoidPrime(a), null, OperationType.Sigmoid);
		}

		public static Operation Sigmoid(dynamic a) => new Operation(a, null, ActivationFunction.Sigmoid(a), ActivationFunction.SigmoidPrime(a), null, OperationType.Sigmoid);
		
		public static Operation ReLU(dynamic a) => new Operation(a, null, ActivationFunction.ReLU(a), ActivationFunction.ReLUPrime(a), null, OperationType.ReLU);

		public static Operation SoftPlus(dynamic a) => new Operation(a, null, ActivationFunction.SoftPlus(a), ActivationFunction.SoftPlusPrime(a), null, OperationType.SoftPlus);

		public static Operation Tanh(dynamic a) => new Operation(a, null, ActivationFunction.Tanh(a), ActivationFunction.TanhPrime(a), null, OperationType.Tanh);

		public static Operation ArcTan(dynamic a) => new Operation(a, null, ActivationFunction.ArcTan(a), ActivationFunction.ArcTanPrime(a), null, OperationType.ArcTan);

		public static Operation CrossEntropyLoss(Operation a, dynamic b)
		{
			if(a.resultType == typeof(Matrix) && b.GetType() == typeof(Matrix))
			{
				Matrix A = (Matrix)a.result;
				Matrix B = (Matrix)b;
				return new Operation(a, b, LossFunction.M_CrossEntropyLoss(A, B), ((B / A) * -1d), (Matrix.Map(A, (dynamic x) => Math.Log(x)) * -1d), OperationType.CrossEntropyLoss);
			}
			return new Operation(a, b, LossFunction.CrossEntropyLoss(a, b), -(b / a), -Math.Log(a), OperationType.CrossEntropyLoss);
		}
		
		public static Operation CrossEntropyLoss(Variable a, dynamic b)
		{
			if(a.type == typeof(Matrix) && b.GetType() == typeof(Matrix))
			{
				Matrix A = (Matrix)a.value;
				Matrix B = (Matrix)b;
				return new Operation(a, b, LossFunction.M_CrossEntropyLoss(A, B), ((B / A) * -1d), (Matrix.Map(A, (dynamic x) => Math.Log(x)) * -1d), OperationType.CrossEntropyLoss);
			}
			return new Operation(a, b, LossFunction.CrossEntropyLoss(a, b), -(b / a), -Math.Log(a), OperationType.CrossEntropyLoss);
		}

		public static Operation CrossEntropyLoss(dynamic a, dynamic b) => new Operation(a, b, LossFunction.CrossEntropyLoss(a, b), -(b / a), -Math.Log(a), OperationType.CrossEntropyLoss);

		public static Operation SquaredError(dynamic a, dynamic b) => new Operation(a, b, LossFunction.SquaredError(a, b), -2d * (b - a), 2d * (b - a), OperationType.SquaredError);

		public static Operation Add(Matrix a, Matrix b) => new Operation(a, b, a + b, new Matrix(a.rows, a.columns, 1d), new Matrix(b.rows, b.columns, 1d), OperationType.Add);

		public static Operation Subtract(Matrix a, Matrix b) => new Operation(a, b, a - b, new Matrix(a.rows, a.columns, 1d), new Matrix(b.rows, b.columns, -1d), OperationType.Subtract);

		public static Operation HadamardProduct(Matrix a, Matrix b) => new Operation(a, b, a * b, b, a, OperationType.HadamardProduct);

		public static Operation MatMul(Operation a, Operation b) => new Operation(a, b, Matrix.MatMul(a.result, b.result), b.result.Transpose(), a.result.Transpose(), OperationType.MatrixMultiply);

		public static Operation MatMul(Operation a, Variable b) => new Operation(a, b, Matrix.MatMul(a.result, b.value), b.value.Transpose(), a.result.Transpose(), OperationType.MatrixMultiply);

		public static Operation MatMul(Operation a, Matrix b) => new Operation(a, b, Matrix.MatMul(a.result, b), b.Transpose(), a.result.Transpose(), OperationType.MatrixMultiply);
		
		public static Operation MatMul(Variable a, Variable b) => new Operation(a, b, Matrix.MatMul(a.value, b.value), b.value.Transpose(), a.value.Transpose(), OperationType.MatrixMultiply);
		
		public static Operation MatMul(Variable a, Operation b) => new Operation(a, b, Matrix.MatMul(a.value, b.result), b.result.Transpose(), a.value.Transpose(), OperationType.MatrixMultiply);
		
		public static Operation MatMul(Variable a, Matrix b) => new Operation(a, b, Matrix.MatMul(a.value, b), b.Transpose(), a.value.Transpose(), OperationType.MatrixMultiply);

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

		public static Operation SquaredError(Operation a, dynamic b)
		{
			if(a.resultType == typeof(Matrix) && b.GetType() == typeof(Matrix))
			{
				Matrix A = (Matrix)a.result;
				Matrix B = (Matrix)b;
				return new Operation(a, b, LossFunction.M_SquaredError(A, B), (B - A) * -2d, (B - A) * 2d, OperationType.SquaredError);
			}
			return new Operation(a, b, LossFunction.SquaredError(a, b), (b - a) * -2d, (b - a) * 2d, OperationType.SquaredError);
		}

		public override string ToString() => $"a = {a},\nb = {b},\nresult = {result},\ndyda = {dyda},\ndydb = {dydb}";

		public static implicit operator int(Operation operation) => (int)operation.result;

		public static implicit operator float(Operation operation) => (float)operation.result;

		public static implicit operator double(Operation operation) => (double)operation.result;

		public static implicit operator long(Operation operation) => (long)operation.result;

		public static implicit operator Matrix(Operation operation) => (Matrix)operation.result;
	}
}
