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
	public Matrix A = new Matrix(1000, 1000);
	public Matrix B = new Matrix(1000, 500);

	[Benchmark]
	public void Test()
	{
		Console.WriteLine(Matrix.MatMul(A, B));
	}
}
