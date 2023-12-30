using JohnFarmer.Mathematics;
using System;
using BenchmarkDotNet.Attributes;
using CommunityToolkit.HighPerformance;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using JohnFarmer.Utility;
using JohnFarmer.Algorithms.NEAT;
using JohnFarmer.NeuralNetwork;
using JohnFarmer.NeuralNetwork.NeuronAndSynapse;

//A class for testing performance of methods/functions.
[MemoryDiagnoser]
public class BenchMark
{
	[Benchmark]
	public void AutoGrad()
	{
		Variable x = new(RandomUtil.Range(-1d, 2d));
		Variable W1 = new(RandomUtil.Range(-1d, 2d), true);
		Variable b1 = new(RandomUtil.Range(-1d, 2d), true);
		Variable W2 = new(RandomUtil.Range(-1d, 2d), true);
		Variable b2 = new(RandomUtil.Range(-1d, 2d), true);
		double l = .5d;
		Operation op_mul1 = Operation.Multiply(W1, x);
		Operation op_add1 = Operation.Add(op_mul1, b1);
		Operation op_act1 = Operation.Tanh(op_add1);

		Operation op_mul2 = Operation.Multiply(W2, op_act1);
		Operation op_add2 = Operation.Add(op_mul2, b2);
		Operation op_act2 = Operation.Tanh(op_add2);

		Operation op_loss = Operation.CrossEntropyLoss(op_act2, 1d);

		AutoGradient.Backward(op_loss);
		W1.value -= W1.gradient * l;
		b1.value -= b1.gradient * l;
		W2.value -= W2.gradient * l;
		b2.value -= b2.gradient * l;
	}
}
