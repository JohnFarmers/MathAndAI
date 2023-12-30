using System;
using System.Collections.Generic;
using JohnFarmer.NeuralNetwork.Matrices;
using JohnFarmer.NeuralNetwork;
using JohnFarmer.Utility;
using BenchmarkDotNet.Running;

public class Program
{
	private readonly static Dictionary<double[], double[]> trainingDatas = new Dictionary<double[], double[]>()
	{
		{ new double[] { 0, 0 }, new double[] { 0 } },
		{ new double[] { 1, 0 }, new double[] { 1 } },
		{ new double[] { 0, 1 }, new double[] { 1 } },
		{ new double[] { 1, 1 }, new double[] { 0 } },
	};

	private static readonly NeuralNetwork nn = new NeuralNetwork(new int[] { 2, 3, 1 }, ActivationFunction.Sigmoid, ActivationFunction.SigmoidPrime, LossFunction.CrossEntropyLoss, .1, .1);
	
	private static void Main(string[] args)
	{
		//BenchmarkRunner.Run<BenchMark>();
		AutoGradTest();
	}

	private static void AutoGradTest()
	{
		Variable x = new(RandomUtil.Range(-1d, 2d));
		Variable W1 = new(RandomUtil.Range(-1d, 2d), true);
		Variable b1 = new(RandomUtil.Range(-1d, 2d), true);
		Variable W2 = new(RandomUtil.Range(-1d, 2d), true);
		Variable b2 = new(RandomUtil.Range(-1d, 2d), true);
		double l = .5d;
		double y_hat = 1d;

		for (int i = 0; i < 1000; i++)
		{
			Operation op_mul1 = Operation.Multiply(W1, x);
			Operation op_add1 = Operation.Add(op_mul1, b1);
			Operation op_act1 = Operation.Sigmoid(op_add1);

			Operation op_mul2 = Operation.Multiply(W2, op_act1);
			Operation op_add2 = Operation.Add(op_mul2, b2);
			Operation op_act2 = Operation.Sigmoid(op_add2);

			Operation op_loss = Operation.CrossEntropyLoss(op_act2, y_hat);

			Console.WriteLine($"=====Iteration {i + 1}======");
			Console.WriteLine("Result: " + op_act2.result);
			Console.WriteLine("Loss: " + op_loss.result);

			AutoGradient.Backward(op_loss);
			W1.value -= W1.gradient * l;
			b1.value -= b1.gradient * l;
			W2.value -= W2.gradient * l;
			b2.value -= b2.gradient * l;
			Console.WriteLine("\n");
		}
	}

	/*private static void CNNTest()
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
		double[] outputs = nn.Predict(O, kernel);
		double maxValue = outputs.Max();
		int maxIndex = outputs.ToList().IndexOf(maxValue);
		Console.WriteLine("This is a picture of " + answers[maxIndex]);

		Console.WriteLine(X.ToMatrix());
		outputs = nn.Predict(X, kernel);
		maxValue = outputs.Max();
		maxIndex = outputs.ToList().IndexOf(maxValue);
		Console.WriteLine("This is a picture of " + answers[maxIndex]);
	}*/

	public void TestNN()
	{
		Console.WriteLine("Pre-train:");
		foreach (double[] input in trainingDatas.Keys)
		{
			string outputs = "[";
			foreach (double d in nn.Forward(input))
				outputs += d + ",";
			Console.WriteLine(input + ": " + outputs.Remove(outputs.Length - 1) + "]");
		}
		Console.WriteLine("\nPost-train:");
		DateTime start = DateTime.Now;
		for (int i = 0; i < 2000; i++)
		{
			foreach (double[] input in trainingDatas.Keys)
				nn.Train(input, trainingDatas[input]);
		}
		DateTime end = DateTime.Now;
		foreach (double[] input in trainingDatas.Keys)
		{
			string outputs = "[";
			foreach (double d in nn.Forward(input))
				outputs += d + ",";
			Console.WriteLine(input + ": " + outputs.Remove(outputs.Length - 1) + "]");
		}
		TimeSpan elasped = end - start;
		Console.WriteLine("\nAccuracy: " + nn.accuracy + "%");
		Console.WriteLine("Elasped training time: " + elasped.TotalMilliseconds + " ms");
	}
}
