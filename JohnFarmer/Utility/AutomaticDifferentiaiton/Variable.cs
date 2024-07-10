using System;
using JohnFarmer.Mathematics;

namespace JohnFarmer.Utility
{
	public class Variable
	{
		public dynamic value;
		public readonly Type type;
		public readonly bool requiredGrad;
		public dynamic gradient;

		public Variable(dynamic value, bool requiredGrad = false)
		{
			this.value = value;
			this.type = value.GetType();
			this.requiredGrad = requiredGrad;
		}

		public void Optimize() => value -= gradient;

		public void Optimize(double learningRate) => value -= gradient * learningRate;

		public static Operation operator +(Variable a, Variable b)
		{
			bool aIsMatrix = a.type == typeof(Matrix);
			bool bIsMatrix = b.type == typeof(Matrix);

			if (aIsMatrix && bIsMatrix)
			{
				Matrix A = (Matrix)a.value;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.value + b.value, new Matrix(row, col, 1d), new Matrix(row, col, 1d), OperationType.Add);
			}
            else if (aIsMatrix)
            {
				Matrix A = (Matrix)a.value;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.value + b.value, new Matrix(row, col, 1d), row * col, OperationType.Add);
			}
			else if (bIsMatrix)
			{
				Matrix B = (Matrix)b.value;
				int row = B.rows;
				int col = B.columns;
				return new Operation(a, b, a.value + b.value, row * col, new Matrix(row, col, 1d), OperationType.Add);
			}

            return new Operation(a, b, a.value + b.value, 1d, 1d, OperationType.Add);
		}

		public static Operation operator +(Variable a, dynamic b)
		{
			bool aIsMatrix = a.type == typeof(Matrix);
			bool bIsMatrix = b.GetType() == typeof(Matrix);

			if (aIsMatrix && bIsMatrix)
			{
				Matrix A = (Matrix)a.value;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.value + b, new Matrix(row, col, 1d), new Matrix(row, col, 1d), OperationType.Add);
			}
			else if (aIsMatrix)
			{
				Matrix A = (Matrix)a.value;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.value + b, new Matrix(row, col, 1d), row * col, OperationType.Add);
			}
			else if (bIsMatrix)
			{
				Matrix B = (Matrix)b;
				int row = B.rows;
				int col = B.columns;
				return new Operation(a, b, a.value + b, row * col, new Matrix(row, col, 1d), OperationType.Add);
			}

			return new Operation(a, b, a.value + b, 1d, 1d, OperationType.Add);
		}

		public static Operation operator -(Variable a, Variable b)
		{
			bool aIsMatrix = a.type == typeof(Matrix);
			bool bIsMatrix = b.type == typeof(Matrix);

			if (aIsMatrix && bIsMatrix)
			{
				Matrix A = (Matrix)a.value;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.value - b.value, new Matrix(row, col, 1d), new Matrix(row, col, -1d), OperationType.Subtract);
			}
			else if (aIsMatrix)
			{
				Matrix A = (Matrix)a.value;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.value - b.value, new Matrix(row, col, 1d), -row * col, OperationType.Subtract);
			}
			else if (bIsMatrix)
			{
				Matrix B = (Matrix)b.value;
				int row = B.rows;
				int col = B.columns;
				return new Operation(a, b, a.value - b.value, row * col, new Matrix(row, col, -1d), OperationType.Subtract);
			}

			return new Operation(a, b, a.value - b.value, 1d, -1d, OperationType.Subtract);
		}

		public static Operation	operator -(Variable a, dynamic b)
		{
			bool aIsMatrix = a.type == typeof(Matrix);
			bool bIsMatrix = b.GetType() == typeof(Matrix);

			if (aIsMatrix && bIsMatrix)
			{
				Matrix A = (Matrix)a.value;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.value - b, new Matrix(row, col, 1d), new Matrix(row, col, -1d), OperationType.Subtract);
			}
			else if (aIsMatrix)
			{
				Matrix A = (Matrix)a.value;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.value - b, new Matrix(row, col, 1d), -row * col, OperationType.Subtract);
			}
			else if (bIsMatrix)
			{
				Matrix B = (Matrix)b;
				int row = B.rows;
				int col = B.columns;
				return new Operation(a, b, a.value - b, row * col, new Matrix(row, col, -1d), OperationType.Subtract);
			}

			return new Operation(a, b, a.value - b, 1d, -1d, OperationType.Subtract);
		}

		public static Operation operator *(Variable a, Variable b)
		{
			bool aIsMatrix = a.type == typeof(Matrix);
			bool bIsMatrix = b.type == typeof(Matrix);

			if (aIsMatrix && bIsMatrix)
			{
				Matrix A = (Matrix)a.value;
				Matrix B = (Matrix)b.value;
				return new Operation(a, b, a.value * b.value, B.Transpose(), A.Transpose(), OperationType.Multiply);
			}
			else if (aIsMatrix)
			{
				Matrix A = (Matrix)a.value;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.value * b.value, new Matrix(row, col, b.value), A.Sum(), OperationType.Multiply);
			}
			else if (bIsMatrix)
			{
				Matrix B = (Matrix)b.value;
				int row = B.rows;
				int col = B.columns;
				return new Operation(a, b, a.value * b.value, B.Sum(), new Matrix(row, col, a.value), OperationType.Multiply);
			}

			return new Operation(a, b, a.value * b.value, b.value, a.value, OperationType.Multiply);
		}
		
		public static Operation operator *(Variable a, Operation b)
		{
			bool aIsMatrix = a.type == typeof(Matrix);
			bool bIsMatrix = b.resultType == typeof(Matrix);

			if (aIsMatrix && bIsMatrix)
			{
				Matrix A = (Matrix)a.value;
				Matrix B = (Matrix)b.result;
				return new Operation(a, b, a.value * b.result, B.Transpose(), A.Transpose(), OperationType.Multiply);
			}
			else if (aIsMatrix)
			{
				Matrix A = (Matrix)a.value;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.value * b.result, new Matrix(row, col, b.result), A.Sum(), OperationType.Multiply);
			}
			else if (bIsMatrix)
			{
				Matrix B = (Matrix)b.result;
				int row = B.rows;
				int col = B.columns;
				return new Operation(a, b, a.value * b.result, B.Sum(), new Matrix(row, col, a.value), OperationType.Multiply);
			}

			return new Operation(a, b, a.value * b.result, b.result, a.value, OperationType.Multiply);
		}

		public static Operation operator *(Variable a, dynamic b)
		{
			bool aIsMatrix = a.type == typeof(Matrix);
			bool bIsMatrix = b.GetType() == typeof(Matrix);

			if (aIsMatrix && bIsMatrix)
			{
				Matrix A = (Matrix)a.value;
				Matrix B = (Matrix)b;
				return new Operation(a, b, a.value * b, B.Transpose(), A.Transpose(), OperationType.Multiply);
			}
			else if (aIsMatrix)
			{
				Matrix A = (Matrix)a.value;
				int row = A.rows;
				int col = A.columns;
				return new Operation(a, b, a.value * b, new Matrix(row, col, b), A.Sum(), OperationType.Multiply);
			}
			else if (bIsMatrix)
			{
				Matrix B = (Matrix)b;
				int row = B.rows;
				int col = B.columns;
				return new Operation(a, b, a.value * b, B.Sum(), new Matrix(row, col, a.value), OperationType.Multiply);
			}

			return new Operation(a, b, a.value * b, b, a.value, OperationType.Multiply);
		}

		public static Operation operator /(Variable a, Variable b) => new Operation(a, b, a.value / b.value, b, a, OperationType.Divide);

		public static Operation operator /(Variable a, dynamic b) => new Operation(a, b, a.value / b, b, a, OperationType.Divide);

		public static Operation operator ^(Variable a, Variable b) => new Operation(a, b, a.value ^ b.value, b, a, OperationType.Power);

		public static Operation operator ^(Variable a, dynamic b) => new Operation(a, b, a.value ^ b, b, a, OperationType.Power);

		public static implicit operator int(Variable variable) => Convert.ChangeType(variable.value, typeof(int));

		public static implicit operator float(Variable variable) => Convert.ChangeType(variable.value, typeof(float));

		public static implicit operator double(Variable variable) => Convert.ChangeType(variable.value, typeof(double));

		public static implicit operator long(Variable variable) => Convert.ChangeType(variable.value, typeof(long));

		public static implicit operator Variable(int a) => new Variable(a);

		public static implicit operator Variable(float a) => new Variable(a);

		public static implicit operator Variable(double a) => new Variable(a);

		public static implicit operator Variable(long a) => new Variable(a);

		public static implicit operator Variable(Matrix a) => new Variable(a);

		public override string ToString() => Convert.ToString(value);
	}
}
