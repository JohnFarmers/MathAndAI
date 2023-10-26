using System;
using System.Collections.Generic;
using MathAndAI.NeuralNetwork;
using System.Linq;
using MathAndAI.Mathematics;

namespace MathAndAI
{
	public class Program
	{
		private readonly static Dictionary<double[], double[]> trainingDatas = new Dictionary<double[], double[]>()
		{
			{ new double[] { 3, 1, 1, 3 }, new double[] { 1, 0 } },
			{ new double[] { 1, 3, 3, 1 }, new double[] { 0, 1 } },
		};

		private static readonly ConvolutionalNeuralNetwork cnn = new ConvolutionalNeuralNetwork(new int[] { 4, 4, 2 }, ActivationFunction.Sigmoid, ActivationFunction.SigmoidPrime, CostFunction.CrossEntropyLoss, .1, .1);

		private static void Main(string[] args)
		{
			TrainNN();
			CNNTest();
		}

		private static void CNNTest()
		{
			double[,] X = new double[6, 6] {
				{ 1, 0, 0, 0, 0, 1 },
				{ 0, 1, 0, 0, 1, 0 },
				{ 0, 0, 1, 1, 0, 0 },
				{ 0, 0, 1, 1, 0, 0 },
				{ 0, 1, 0, 0, 1, 0 },
				{ 1, 0, 0, 0, 0, 1 },
			};

			double[,] O = new double[6, 6] {
				{ 0, 0, 1, 1, 0, 0 },
				{ 0, 1, 0, 0, 1, 0 },
				{ 1, 0, 0, 0, 0, 1 },
				{ 1, 0, 0, 0, 0, 1 },
				{ 0, 1, 0, 0, 1, 0 },
				{ 0, 0, 1, 1, 0, 0 },
			};

			double[,] kernel = new double[3, 3] {
				{ 0, 0, 1 },
				{ 0, 1, 0 },
				{ 1, 0, 0 }
			};

			string[] answers = new string[] 
			{
				"O",
				"X"
			};

			Console.WriteLine(O.ToMatrix());
			double[] outputs = cnn.Predict(O, kernel);
			double maxValue = outputs.Max();
			int maxIndex = outputs.ToList().IndexOf(maxValue);
			Console.WriteLine("This is a picture of " + answers[maxIndex]);

			Console.WriteLine(X.ToMatrix());
			outputs = cnn.Predict(X, kernel);
			maxValue = outputs.Max();
			maxIndex = outputs.ToList().IndexOf(maxValue);
			Console.WriteLine("This is a picture of " + answers[maxIndex]);
		}

		public static void TrainNN()
		{
			Console.WriteLine("Pre-train:");
			foreach (double[] input in trainingDatas.Keys)
			{
				string outputs = "[";
				foreach (double d in cnn.Predict(input))
					outputs += d + ",";
				Console.WriteLine(input + ": " + outputs.Remove(outputs.Length - 1) + "]");
			}
			Console.WriteLine("\nPost-train:");
			DateTime start = DateTime.Now;
			for (int i = 0; i < 1000; i++)
			{
				if (cnn.accuracy >= 75)
					break;
				foreach (double[] input in trainingDatas.Keys)
					cnn.Train(input, trainingDatas[input]);
			}
			DateTime end = DateTime.Now;
			foreach (double[] input in trainingDatas.Keys)
			{
				string outputs = "[";
				foreach (double d in cnn.Predict(input))
					outputs += d + ",";
				Console.WriteLine(input + ": " + outputs.Remove(outputs.Length - 1) + "]");
			}
			TimeSpan elasped = end - start;
			Console.WriteLine("\nAccuracy: " + cnn.accuracy + "%");
			Console.WriteLine("Elasped training time: " + elasped.TotalMilliseconds + " ms");
		}
	}
}
