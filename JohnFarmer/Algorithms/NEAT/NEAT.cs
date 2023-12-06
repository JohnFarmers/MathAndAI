using System;
using System.Collections.Generic;
using JohnFarmer.NeuralNetwork.NeuronAndSynapse;
using CommunityToolkit.HighPerformance;
using JohnFarmer.Utility;

namespace JohnFarmer.Algorithms.NEAT
{
	[Serializable]
	public class NEAT : NeuralNetwork.NeuronAndSynapse.NeuralNetwork
	{
		public NEAT(int[] layerNodes, Func<double, double> activationFunction) : base(layerNodes, activationFunction) { }

		public virtual Neuron AddNeuron(int layerIndex, List<Neuron> inNeurons, List<Neuron> outNeurons)
		{
			if(layerIndex == 0 || layerIndex == layers.Count - 1)
				throw new Exception("Adding neuron to input or output layer is not allowed");
			Neuron neuron = new Neuron(RandNum(), activationFunction);
			Span<Neuron> inNeuronsSpan = inNeurons.AsSpan();
			for (int i = 0; i < inNeuronsSpan.Length; i++)
			{
				Synapse synapse = new Synapse(RandNum(), inNeuronsSpan[i], neuron);
				neuron.inSynapses.Add(synapse);
				inNeuronsSpan[i].outSynapses.Add(synapse);
				synapses.Add(synapse);
			}
			Span<Neuron> outNeuronsSpan = outNeurons.AsSpan();
			for (int i = 0; i < outNeuronsSpan.Length; i++)
			{
				Synapse synapse = new Synapse(RandNum(), neuron, outNeuronsSpan[i]);
				neuron.outSynapses.Add(synapse);
				outNeuronsSpan[i].inSynapses.Add(synapse);
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

		public virtual void MutateRandomWeight(int amount)
		{
			for (int i = 0; i < amount; i++)
				synapses.GetRandom().weight = RandNum() * RandomUtil.Range(1, 10);
		}

		public virtual void MutateRandomBias(int amount)
		{
			for (int i = 0; i < amount; i++)
				layers.GetRandom().GetRandom().bias = RandNum() * RandomUtil.Range(1, 10);
		}

		public virtual void Mutate()
		{
			int rng = RandomUtil.Range(0, 5);
			int layer = RandomUtil.Range(1, layerNodes.Length - 1);
			switch (rng)
			{
				case 0:
					AddNeuron(layer, GetRandomNeurons(layer, true), GetRandomNeurons(layer, false));
					break;
				case 1:
					if (layers[layer].Count > 1)
						RemoveNeuron(layer, layers[layer].GetRandom());
					break;
				case 2:
					AddSynapse(layers[layer].GetRandom(), layers[RandomUtil.Range(layer + 1, layers.Count)].GetRandom());
					break;
				case 3:
					if (synapses.Count > 1)
						RemoveSynapse(synapses.GetRandom());
					break;
				case 4:
					MutateRandomWeight(RandomUtil.Range(1, 11));
					break;
				case 5:
					MutateRandomBias(RandomUtil.Range(1, 11));
					break;
				default:
					break;
			}

			List<Neuron> GetRandomNeurons(int currentLayer, bool getFromLeft)
			{
				List<Neuron> neurons = new List<Neuron>();
				int layer = RandomUtil.Range(getFromLeft ? 0 : currentLayer + 1, getFromLeft ? currentLayer - 1 : layerNodes.Length);
				int amount = RandomUtil.Range(0, layerNodes[layer] - 1);

				for (int i = 0; i < amount; i++)
					neurons.Add(layers[layer].GetRandom());

				return neurons;
			}
		}

		public static NEAT Crossover(NEAT a, NEAT b)
		{
			return null; //TODO
		}
	}
}
