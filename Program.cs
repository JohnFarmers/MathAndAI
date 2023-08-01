using System;
using System.Collections.Generic;
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

		private static readonly NeuralNetwork neuralNetwork = new NeuralNetwork(new int[] { 2, 2, 1 }, ActivationFunction.Sigmoid, ActivationFunction.SigmoidPrime, CostFunction.CrossEntropyLoss, .1, .1);

		private static void Main(string[] args)
		{
			Console.WriteLine("Pre-train:");
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
			Console.WriteLine("\nPost-train:");
			DateTime start = DateTime.Now;
			for (int i = 0; i < 1000; i++)
			{
				if (neuralNetwork.accuracy >= 75)
					break;
				foreach (double[] input in trainingDatas.Keys)
					neuralNetwork.Train(input, trainingDatas[input]);
			}
			DateTime end = DateTime.Now;
			foreach (double[] input in trainingDatas.Keys)
			{
				string outputs = "[";
				foreach (double d in neuralNetwork.Predict(input))
					outputs += d + ",";
				Console.WriteLine(input + ": " + outputs.Remove(outputs.Length - 1) + "]");
			}
			TimeSpan elasped = end - start;
			Console.WriteLine("\nAccuracy: " + neuralNetwork.accuracy + "%");
			Console.WriteLine("Elasped training time: " + elasped.TotalMilliseconds + " ms");
		}
	}
}
