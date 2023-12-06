using CommunityToolkit.HighPerformance;
using JohnFarmer.Utility;
using System;
using System.Collections.Generic;

namespace JohnFarmer.NeuralNetwork.NeuronAndSynapse
{
	[Serializable]
	public class NeuralNetwork
	{
		public readonly int[] layerNodes;
		public Func<double, double> activationFunction;
		public readonly List<List<Neuron>> layers;
		public readonly List<Synapse> synapses;
		private readonly int inputAmount;
		public bool Initialized { get; private set; } = false;

		public NeuralNetwork(int[] layerNodes, Func<double, double> activationFunction)
		{
			this.layerNodes = layerNodes;
			this.activationFunction = activationFunction;
			layers = new List<List<Neuron>>();
			synapses = new List<Synapse>();
			inputAmount = layerNodes[0];
			Initialize();
		}

		public virtual void Initialize()
		{
			for (int i = 0; i < layerNodes.Length; i++)
			{
				int length = layerNodes[i];
				List<Neuron> layer = new List<Neuron>();
				bool isInputLayer = i == 0;
				bool isOutputLayer = i == layerNodes.Length - 1;
				for (int j = 0; j < length; j++)
					layer.Add(new Neuron(isInputLayer ? 0 : RandNum(), activationFunction, isInputLayer, isOutputLayer));
				layers.Add(layer);
			}

			for (int i = 0; i < layerNodes.Length - 1; i++)
			{
				Span<Neuron> currentLayer = layers[i].AsSpan();
				Span<Neuron> nextLayer = layers[i + 1].AsSpan();
				for (int j = 0; j < currentLayer.Length; j++)
				{
					for (int k = 0; k < nextLayer.Length; k++)
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
			Initialized = true;
		}

		public virtual double[] Forward(double[] inputs)
		{
			if (inputs.Length != inputAmount)
				throw new Exception("The amount of inputs does not match the amount of nodes in input layer.");
			Span<double> doubles = inputs.AsSpan();
			for (int i = 0; i < doubles.Length; i++)
				layers[0][i].input = doubles[i];
			layers.ForEach(layer => layer.ForEach(neuron => neuron.Evaluate()));
			Span<Neuron> outputLayer = layers[^1].AsSpan();
			double[] outputs = new double[outputLayer.Length];
			for (int i = 0; i < outputLayer.Length; i++)
				outputs[i] = outputLayer[i].Output;
			return outputs;
		}

		public virtual void Train(double[] inputs, double[] targetOutputs) 
		{ 
			//TODO
		}

		protected double RandNum() => RandomUtil.Range(-1d, 1d);
	}
}
