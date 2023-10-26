namespace MathAndAI.Mathematics
{
	public static class MatrixExtension
	{
		public static Matrix ToMatrix(this double[,] array)
		{
			int rows = array.GetLength(0);
			int columns = array.GetLength(1);
			Matrix matrix = new Matrix(array.GetLength(0), array.GetLength(1));
			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					matrix[row, column] = array[row, column];
			return matrix;
		}

		public static Matrix To1DMatrix(this double[] array)
		{
			Matrix matrix = new Matrix(array.Length, 1);
			for (int i = 0; i < array.Length; i++)
				matrix[i, 0] = array[i];
			return matrix;
		}
	}
}
