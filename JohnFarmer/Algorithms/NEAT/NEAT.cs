using System;
using System.Collections.Generic;

namespace JohnFarmer.Algorithms.NEAT
{
	public class NEAT
	{
		public int[] layerNodes;
		public readonly Func<double, double> activationFunction;
		public readonly List<List<Neuron>> layers;
		public readonly List<Synapse> synapses;
		private readonly Random random;

		public NEAT(int[] layerNodes, Func<double, double> activationFunction)
		{
			this.layerNodes = layerNodes;
			this.activationFunction = activationFunction;
			layers = new List<List<Neuron>>();
			synapses = new List<Synapse>();
			random = new Random();
			Initialize();
		}

		public virtual void Initialize()
		{
			for (int i = 0; i < layerNodes.Length; i++)
			{
				int length = layerNodes[i];
				List<Neuron> layer = new List<Neuron>();
				bool isInputLayer = i == 0; 
				bool isOutputLayer =  i == layerNodes.Length - 1;
				for (int j = 0; j < length; j++)
					layer.Add(new Neuron(isInputLayer ? 0 : RandNum(), activationFunction, isInputLayer, isOutputLayer));
				layers.Add(layer);
			}

			for (int i = 0; i < layerNodes.Length - 1; i++)
			{
				List<Neuron> currentLayer = layers[i];
				List<Neuron> nextLayer = layers[i + 1];
				for (int j = 0; j < currentLayer.Count; j++)
				{
					for (int k = 0; k < nextLayer.Count; k++)
					{
						Neuron hiddenNeuron = currentLayer[j];
						Neuron nextHiddenNeuron = nextLayer[k];
						Synapse synapse = new Synapse(RandNum(), hiddenNeuron, nextHiddenNeuron);
						hiddenNeuron.outSynapses.Add(synapse);
						nextHiddenNeuron.inSynapses.Add(synapse);
						synapses.Add(synapse);
					}
				}
			}
		}

		public virtual double[] Forward(double[] inputs)
		{
			for (int i = 0; i < inputs.Length; i++)
				layers[0][i].input = inputs[i];
			layers.ForEach(layer => layer.ForEach(neuron => neuron.Evaluate()));
			List<Neuron> outputLayer = layers[^1];
			double[] outputs = new double[outputLayer.Count];
			for (int i = 0; i < outputLayer.Count; i++)
				outputs[i] = outputLayer[i].output;
			return outputs;
		}

		public virtual Neuron AddNeuron(int layerIndex, List<Neuron> inNeuron, List<Neuron> outNeuron)
		{
			if(layerIndex == 0 || layerIndex == layers.Count - 1)
				throw new Exception("Adding neuron to input or output layer is not allowed");
			Neuron neuron = new Neuron(RandNum(), activationFunction);
			for (int i = 0; i < inNeuron.Count; i++)
			{
				Synapse synapse = new Synapse(RandNum(), inNeuron[i], neuron);
				neuron.inSynapses.Add(synapse);
				inNeuron[i].outSynapses.Add(synapse);
				synapses.Add(synapse);
			}
			for (int i = 0; i < outNeuron.Count; i++)
			{
				Synapse synapse = new Synapse(RandNum(), neuron, outNeuron[i]);
				neuron.outSynapses.Add(synapse);
				outNeuron[i].inSynapses.Add(synapse);
				synapses.Add(synapse);
			}
			layers[layerIndex].Add(neuron);
			layerNodes[layerIndex]++;
			return neuron;
		}
		
		public virtual void RemoveNeuron(int layerIndex, Neuron neuron)
		{
			if (layerIndex == 0 || layerIndex == layers.Count - 1)
				throw new Exception("Removing neuron in the input or output layer is not allowed");
			for (int i = 0; i < neuron.inSynapses.Count; i++)
			{
				Synapse synapse = neuron.inSynapses[i];
				synapse.Remove();
				synapses.Remove(synapse);
			}
			for (int i = 0; i < neuron.outSynapses.Count; i++)
			{
				Synapse synapse = neuron.outSynapses[i];
				synapse.Remove();
				synapses.Remove(synapse);
			}
			neuron.Remove();
			layers[layerIndex].Remove(neuron);
			layerNodes[layerIndex]--;
		}

		public virtual Synapse AddSynapse(Neuron inNeuron, Neuron outNeuron)
		{
			Synapse synapse = new Synapse(RandNum(), inNeuron, outNeuron);
			inNeuron.outSynapses.Add(synapse);
			outNeuron.inSynapses.Add(synapse);
			synapses.Add(synapse);
			return synapse;
		}

		public virtual void RemoveSynapse(Synapse synapse)
		{
			synapse.Remove();
			synapses.Remove(synapse);
		}

		public void Mutate()
		{
			//TODO
		}

		public static NEAT Crossover(NEAT a, NEAT b)
		{
			return null; //Temp
		}

		private double RandNum() => random.NextDouble() * 2 - 1;
	}
}
