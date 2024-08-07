﻿using System;
using System.Collections.Generic;
using JohnFarmer.NeuralNetwork.Matrices;
using JohnFarmer.NeuralNetwork;
using JohnFarmer.Utility;
using BenchmarkDotNet.Running;
using JohnFarmer.Mathematics;
using MathAndAI.JohnFarmer.Utility;

public class Program
{
	private readonly static Dictionary<double[], double[]> trainingDatas = new Dictionary<double[], double[]>()
	{
		{ new double[] { 0, 0 }, new double[] { 0 } },
		{ new double[] { 1, 0 }, new double[] { 1 } },
		{ new double[] { 0, 1 }, new double[] { 1 } },
		{ new double[] { 1, 1 }, new double[] { 0 } },
	};

	private static readonly NeuralNetwork nn = new NeuralNetwork(new int[] { 2, 3, 1 }, ActivationFunction.Sigmoid, ActivationFunction.SigmoidPrime, LossFunction.CrossEntropyLoss, .5d);
	
	private static void Main(string[] args)
	{
		//BenchmarkRunner.Run<BenchMark>();

		W2VTest();
	}

	public static void W2VTest()
	{
		Word2Vec w2v = new Word2Vec(
			new string[] {
				"Apple is fruit",
				"Orange is fruit",
				"Banana is fruit",
				"Mango is fruit",
				"Fruit is good for your health",
				"Apple is one type of fruit",
				"Orange is a type of fruit",
				"Mango is one type of fruit",
				"Banana is a type of fruit",
				"Apple is one kind of fruit",
				"Orange is one kind of fruit",
				"Mango is a kind of fruit",
				"Banana is one kind of fruit",
				"Your health will get better if you eat many type of fruit",
				"Your health will get better if you eat many kind of vegetable",
				"Better eat more fruit",
				"Many type of fruit are good",
				"Fruit are not vegetable",
				"Fruit is a food",
				"Fruit is one kind of food",
				"Human needs to eat food to survive",
				"Human needs to eat many food to survive",
				"Human needs to eat fruit for better health",
				"Vegetable also make human healthy",
				"Vegetable is a healthy food",
				"Vegetable is one kind of food",
				"Vegetable is one type of food",
				"Vegetable is also a healthy food",
				"Fruit is a healthy food",
				"Fruit is also a healthy food",
				"Apple is not a type of vegetable",
				"Orange is not a type of vegetable",
				"Mango is not a type of vegetable",
				"Banana is not a type of vegetable",
				"Fruit is not a vegetable",
				"Vegetable come from plant",
				"Fruit come from plant",
				"Vegetable also come from plant",
				"Fruit also from plant",
				"Fruit cannot eat human",
				"Vegetable cannot eat human",
				"Human cannot eat human",
				"Human can eat fruit",
				"Human can eat vegetable",
				"Human can eat food",
				"Human can eat many food",
			},
			2
		);

		DateTime start = DateTime.Now;

		for (int i = 0; i < 1000; i++)
		{
			w2v.Train();
			Console.WriteLine(w2v.weights1.ToString());
		}

		DateTime end = DateTime.Now;
		TimeSpan elasped = end - start;
		Console.WriteLine("Elasped training time: " + elasped.TotalMilliseconds + " ms");

		w2v.ShowPlot();
	}

	public static double[] SoftMax(double[] inputs)
	{
		int length = inputs.Length;
		double sum = 0d;

		for (int i = 0; i < length; i++)
			sum += Math.Exp(inputs[i]);

		double[] results = new double[length];

		for (int i = 0; i < length; i++)
			results[i] = Math.Exp(inputs[i]) / sum;

		return results;
	}

	private static void LSTMTest()
	{
		double[] inputs = new double[] { 0d, .25d, .5d, .75d };
		//double[] inputs2 = new double[] { 1d, .5d, .25d };
		LSTM lstm = new(LossFunction.SquaredError, .1d);
		Console.WriteLine("Pre-train:");
		double output = lstm.Forward(inputs);
		Console.WriteLine(output);
		/*output = lstm.Forward(inputs2);
		Console.WriteLine(output);*/
		for (int i = 0; i < 5000; i++)
			lstm.Train(inputs, 1d);
		/*for (int i = 0; i < 3000; i++)
			lstm.Train(inputs2, 0d);*/
		Console.WriteLine("\nPost-train:");
		output = lstm.Forward(inputs);
		Console.WriteLine(output);
		/*output = lstm.Forward(inputs2);
		Console.WriteLine(output);*/
	}

	private static void AutoGradTest()
	{
		Variable x = new Matrix(2, 1).Randomize();
		Variable W1 = new(new Matrix(3, 2).Randomize(), true);
		Variable b1 = new(new Matrix(3, 1).Randomize(), true);
		Variable W2 = new(new Matrix(5, 3).Randomize(), true);
		Variable b2 = new(new Matrix(5, 1).Randomize(), true);
		double l = .1d;
		Matrix y_hat = new Matrix(
			new double[,] {
				{ 1 },
				{ 0 },
				{ 1 },
				{ 0 },
				{ 1 },
			}
		);

		for (int i = 0; i < 500; i++)
		{
			Operation op_mul1 = Operation.MatMul(W1, x);
			Operation op_add1 = op_mul1 + b1;
			Operation op_act1 = Operation.Sigmoid(op_add1);
			
			Operation op_mul2 = Operation.MatMul(W2, op_act1);
			Operation op_add2 = op_mul2 + b2;
			Operation op_act2 = Operation.Sigmoid(op_add2);

			Operation op_loss = Operation.SquaredError(op_act2, y_hat);

			Console.WriteLine($"=====Iteration {i + 1}======");
			Console.WriteLine("Result: " + op_act2.result);
			Console.WriteLine("Loss: " + op_loss.result);

			op_loss.Backward();
			W2.Optimize(l);
			b2.Optimize(l);
			W1.Optimize(l);
			b1.Optimize(l);
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

	public static void TestNN()
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
		for (int i = 0; i < 1000; i++)
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
		Console.WriteLine("Elasped training time: " + elasped.TotalMilliseconds + " ms");
	}
}
