using CommunityToolkit.HighPerformance;
using System;
using System.Collections.Generic;

namespace JohnFarmer.NeuralNetwork.NeuronAndSynapse
{
	[Serializable]
	public class Neuron
	{
		public double input;
		public double bias;
		public Func<double, double> activationFunction;
		public List<Synapse> inSynapses;
		public List<Synapse> outSynapses;
		public readonly bool isInputLayer;
		public readonly bool isOutputLayer;
		public double Output { get; private set; }

		public Neuron(double bias, Func<double, double> activationFunction, bool isInputLayer = false, bool isOutputLayer = false)
		{
			this.bias = bias;
			this.activationFunction = activationFunction;
			this.isInputLayer = isInputLayer;
			this.isOutputLayer = isOutputLayer;
			inSynapses = new List<Synapse>();
			outSynapses = new List<Synapse>();
		}

		public double Evaluate()
		{
			double weightedSum = isInputLayer ? input : 0;
			Span<Synapse> synapses = inSynapses.AsSpan();
			for (int i = 0; i < synapses.Length; i++)
				weightedSum += synapses[i].weight * synapses[i].inNeuron.Output;
			Output = activationFunction(weightedSum + bias);
			return Output;
		}

		public void Remove()
		{
			inSynapses.Clear();
			outSynapses.Clear();
		}
	}
}
