using System;
using JohnFarmer.Mathematics;

namespace JohnFarmer.NeuralNetwork
{
	public struct LossFunction
	{
		public static Func<dynamic, dynamic, dynamic> CrossEntropyLoss = (dynamic x, dynamic y) => -(y * Math.Log(x));
		public static Func<dynamic, dynamic, dynamic> SquaredError = (dynamic x, dynamic y) => Math.Pow(y - x, 2);
		public static Func<Matrix, Matrix, Matrix> M_CrossEntropyLoss = (Matrix x, Matrix y) => (y * Matrix.Map(x, (dynamic a) => Math.Log(a))) * -1;
		public static Func<Matrix, Matrix, Matrix> M_SquaredError = (Matrix x, Matrix y) => (x - y).Map((dynamic a) => Math.Pow(a, 2));
	}
}
