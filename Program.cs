using System;
using System.Collections.Generic;
using JohnFarmer.Mathematics;
using JohnFarmer.NeuralNetwork;

namespace TestingConsole
{
	public class Program
	{
		private readonly static Dictionary<double[], double[]> trainingDatas = new Dictionary<double[], double[]>()
		{
			{ new double[] { 0, 0 }, new double[] { 0 } },
			{ new double[] { 1, 0 }, new double[] { 1 } },
			{ new double[] { 0, 1 }, new double[] { 1 } },
			{ new double[] { 0, 0 }, new double[] { 0 } }
		};

		private static readonly NeuralNetwork neuralNetwork = new NeuralNetwork(2, 3, 2, 1);

		private static void Main(string[] args)
		{
			foreach (double[] input in trainingDatas.Keys)
			{
				string outputs = "[";
				foreach (double d in neuralNetwork.Predict(input))
					outputs += d + ",";
				Console.WriteLine(input + ": " + outputs.Remove(outputs.Length - 1) + "]");
			}
			TestNeuralNetwork();
		}

		public static void TestNeuralNetwork()
		{
			for (int i = 0; i < 10000; i++)
				foreach (double[] input in trainingDatas.Keys)
					neuralNetwork.Train(input, trainingDatas[input]);
			foreach (double[] input in trainingDatas.Keys)
			{
				string outputs = "[";
				foreach (double d in neuralNetwork.Predict(input))
					outputs += d + ",";
				Console.WriteLine(input + ": " + outputs.Remove(outputs.Length - 1) + "]");
			}
		}

		public static void Matrix(int rows, int columns) => Console.WriteLine(new Matrix(rows, columns).Randomize());

		public static void TransposeMatrix(int rows, int columns)
		{
			Matrix matrix = new Matrix(rows, columns).Randomize();
			Console.WriteLine(matrix);
			Console.WriteLine(matrix.Transpose());
			Console.WriteLine(matrix.Transpose().Transpose());
		}
	}
}
