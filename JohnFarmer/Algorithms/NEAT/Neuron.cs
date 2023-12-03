using System;
using System.Collections.Generic;

namespace JohnFarmer.Algorithms.NEAT
{
	public class Neuron
	{
		public double input;
		public double bias;
		public Func<double, double> activationFunction;
		public List<Synapse> inSynapses;
		public List<Synapse> outSynapses;
		public double output;
		public readonly bool isInputLayer;
		public readonly bool isOutputLayer;

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
			inSynapses.ForEach(s => weightedSum += s.weight * s.inNeuron.output);
			output = activationFunction(weightedSum + bias);
			return output;
		}

		public void Remove()
		{
			inSynapses.Clear();
			outSynapses.Clear();
		}
	}
}
