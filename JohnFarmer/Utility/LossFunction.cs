using System;
using JohnFarmer.Mathematics;

namespace JohnFarmer.NeuralNetwork
{
	public struct LossFunction
	{
		public static Func<double, double, double> CrossEntropyLoss = (double x, double y) => -(y * Math.Log(x));
		public static Func<double, double, double> SquaredError = (double x, double y) => Math.Pow(y - x, 2);
		public static Func<Matrix, Matrix, Matrix> M_CrossEntropyLoss = (Matrix x, Matrix y) => Matrix.HadamardProduct(y, x.Map(Math.Log)) * -1;
		public static Func<Matrix, Matrix, Matrix> M_SquaredError = (Matrix x, Matrix y) => (x - y).Map((double a) => Math.Pow(a, 2));
	}
}
