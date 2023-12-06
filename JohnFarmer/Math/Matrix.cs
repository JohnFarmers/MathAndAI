using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;
using JohnFarmer.Utility;

namespace JohnFarmer.Mathematics
{
	public struct Matrix
	{
		public readonly int rows, columns;
		public readonly double[,] values;

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

		public Matrix this[Range rowRange, Range columnRange]
		{
			get {
				int rowStart = rowRange.Start.Value;
				int rowEnd = rowRange.End.Value;
				int columnStart = columnRange.Start.Value;
				int columnEnd = columnRange.End.Value;
				Matrix result = new Matrix(rowEnd - rowStart, columnEnd - columnStart);
				for (int row = 0; row < result.rows; row++)
					for (int column = 0; column < result.columns; column++)
						result[row, column] = values[row + rowStart, column + columnStart];
				return result;
			}
		}

		public Matrix Randomize()
		{
			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					values[row, column] = RandomUtil.Range(-1d, 1d);
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
			Span<double> span = values.AsSpan();
			ref var start = ref MemoryMarshal.GetReference(span);
			ref var end = ref Unsafe.Add(ref start, span.Length);

			while (Unsafe.IsAddressLessThan(ref start, ref end))
			{
				doubles.Add(start);
				start = ref Unsafe.Add(ref start, 1);
			}

			return doubles.ToArray();
		}
		
		public double[,] To2DArray()
		{
			double[,] doubles = new double[rows, columns];
			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					doubles[row, column] = values[row, column];
			return doubles;
		}

		public double GetSum()
		{
			double sum = 0;

			Span<double> span = values.AsSpan();
			ref var start = ref MemoryMarshal.GetReference(span);
			ref var end = ref Unsafe.Add(ref start, span.Length);

			while (Unsafe.IsAddressLessThan(ref start, ref end))
			{
				sum += start;
				start = ref Unsafe.Add(ref start, 1);
			}

			return sum;
		}

		public override string ToString()
		{
			string text = string.Empty;
			for (int row = 0; row < rows; row++)
			{
				text += "\n| ";
				for (int column = 0; column < columns; column++)
				{
					double value = values[row, column];
					string space = value >= 0 ? " " : "";
					text += value.ToString($"{space}0.00") + " ";
				}
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

			bool useUnsafe = matrix1.rows * matrix1.columns <= 300;
			var matrix1Span = matrix1.values.AsSpan2D();
			var matrix2Span = useUnsafe ? matrix2.Transpose().values.AsSpan2D() : matrix2.values.AsSpan2D();

			for (int row = 0; row < product.rows; row++)
			{
				for (int column = 0; column < product.columns; column++)
				{
					double sum = 0;
					if (useUnsafe)
					{
						Span<double> span1 = matrix1Span.GetRowSpan(row);
						ref var start1 = ref MemoryMarshal.GetReference(span1);
						ref var end1 = ref Unsafe.Add(ref start1, span1.Length);

						Span<double> span2 = matrix2Span.GetRowSpan(column);
						ref var start2 = ref MemoryMarshal.GetReference(span2);
						ref var end2 = ref Unsafe.Add(ref start2, span2.Length);

						while (Unsafe.IsAddressLessThan(ref start1, ref end1) && Unsafe.IsAddressLessThan(ref start2, ref end2))
						{
							sum += start1 * start2;
							start1 = ref Unsafe.Add(ref start1, 1);
							start2 = ref Unsafe.Add(ref start2, 1);
						}
					}
					else
					{
						for (int i = 0; i < matrix1.columns; i++)
							sum += matrix1Span[row, i] * matrix2Span[i, column];
					}
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
