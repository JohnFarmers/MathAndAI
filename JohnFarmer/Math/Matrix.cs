using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JohnFarmer.Mathematics
{
	public struct Matrix
	{
		public readonly int rows, columns;
		private readonly double[,] values;

		public Matrix(int rows, int columns)
		{
			this.rows = rows;
			this.columns = columns;
			values = new double[rows, columns];
			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					values[row, column] = 0;
		}

		public double this[int row, int column] { get => values[row, column]; set => values[row, column] = value; }

		public Matrix Randomize()
		{
			Random random = new Random();
			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					values[row, column] = random.NextDouble() * 2 - 1;
			return this;
		}

		public Matrix Transpose()
		{
			Matrix result = new Matrix(columns, rows);
			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					result[column, row] = values[row, column];
			return result;
		}

		public Matrix Map(Func<double, double> function)
		{
			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					values[row, column] = function.Invoke(values[row, column]);
			return this;
		}

		public double[] ToArray()
		{
			List<double> doubles = new List<double>();
			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					doubles.Add(values[row, column]);
			return doubles.ToArray();
		}

		public override string ToString()
		{
			string text = string.Empty;
			for (int row = 0; row < rows; row++)
			{
				text += "\n| ";
				for (int column = 0; column < columns; column++)
					text += values[row, column] + " ";
				text += "|";
			}
			return text;
		}

		public static Matrix Map(Matrix matrix, Func<double, double> function)
		{
			Matrix result = new Matrix(matrix.rows, matrix.columns);
			for (int row = 0; row < result.rows; row++)
				for (int column = 0; column < result.columns; column++)
					result[row, column] = function.Invoke(matrix[row, column]);
			return result;
		}

		public static Matrix Power(Matrix matrix, int power)
		{
			Matrix result = matrix;
			for (int i = 0; i < power - 1; i++)
				result = HadamardProduct(result, matrix);
			return result;
		}

		public static Matrix HadamardProduct(Matrix matrix1, Matrix matrix2)
		{
			if (matrix1.columns != matrix2.columns || matrix1.rows != matrix2.rows)
				throw new Exception("The dimension of the matrix must match.");
			Matrix result = new Matrix(matrix1.rows, matrix1.columns);
			for (int row = 0; row < result.rows; row++)
				for (int column = 0; column < result.columns; column++)
					result[row, column] = matrix1[row, column] * matrix2[row, column];
			return result;
		}

		public static Matrix operator *(Matrix matrix, double multiplier)
		{
			Matrix result = new Matrix(matrix.rows, matrix.columns);
			for (int row = 0; row < result.rows; row++)
				for (int column = 0; column < result.columns; column++)
					result[row, column] = matrix[row, column] * multiplier;
			return result;
		}

		public static Matrix operator *(Matrix matrix1, Matrix matrix2)
		{
			if (matrix1.columns != matrix2.rows)
				throw new Exception("Columns of matrix1 must match rows of matrix2.");
			Matrix product = new Matrix(matrix1.rows, matrix2.columns);
			for (int row = 0; row < product.rows; row++)
			{
				for (int column = 0; column < product.columns; column++)
				{
					double sum = 0;
					for (int i = 0; i < matrix1.columns; i++)
						sum += matrix1[row, i] * matrix2[i, column];
					product[row, column] = sum;
				}
			}
			return product;
		}

		public static Matrix operator +(Matrix matrix, double value)
		{
			Matrix result = new Matrix(matrix.rows, matrix.columns);
			for (int row = 0; row < result.rows; row++)
				for (int column = 0; column < result.columns; column++)
					result[row, column] = matrix[row, column] + value;
			return result;
		}

		public static Matrix operator +(Matrix matrix1, Matrix matrix2)
		{
			if (matrix1.rows != matrix2.rows || matrix1.columns != matrix2.columns)
				throw new Exception("The dimension of the matrix must match.");
			Matrix result = new Matrix(matrix1.rows, matrix1.columns);
			for (int row = 0; row < result.rows; row++)
				for (int column = 0; column < result.columns; column++)
					result[row, column] = matrix1[row, column] + matrix2[row, column];
			return result;
		}

		public static Matrix operator -(Matrix matrix, double value)
		{
			Matrix result = new Matrix(matrix.rows, matrix.columns);
			for (int row = 0; row < result.rows; row++)
				for (int column = 0; column < result.columns; column++)
					result[row, column] = matrix[row, column] - value;
			return result;
		}

		public static Matrix operator -(Matrix matrix1, Matrix matrix2)
		{
			if (matrix1.rows != matrix2.rows || matrix1.columns != matrix2.columns)
				throw new Exception("The dimension of the matrix must match.");
			Matrix result = new Matrix(matrix1.rows, matrix1.columns);
			for (int row = 0; row < result.rows; row++)
				for (int column = 0; column < result.columns; column++)
					result[row, column] = matrix1[row, column] - matrix2[row, column];
			return result;
		}
	}
}
