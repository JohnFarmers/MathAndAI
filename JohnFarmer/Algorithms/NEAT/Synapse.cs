using System;

namespace JohnFarmer.NeuralNetwork.NeuronAndSynapse
{
	[Serializable]
	public class Synapse
	{
		public double weight = 1;
		public Neuron inNeuron;
		public Neuron outNeuron;

		public Synapse(double weight, Neuron inNeuron, Neuron outNeuron)
		{
			this.weight = weight;
			this.inNeuron = inNeuron;
			this.outNeuron = outNeuron;
		}

		public void Remove()
		{
			inNeuron.outSynapses.Remove(this);
			outNeuron.inSynapses.Remove(this);
		}
	}
}
