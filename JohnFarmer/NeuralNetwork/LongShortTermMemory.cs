using JohnFarmer.NeuralNetwork;
using System;
using System.Collections.Generic;

namespace TestingConsole.JohnFarmer.NeuralNetwork
{
	public class LongShortTermMemory
	{
		public double
				forgetGateWeight,
				forgetGateUWeight,
				forgetGateBias,
				inputGateWeight,
				inputGateUWeight,
				inputGateBias,
				cellStateUpdateWeight,
				cellStateUpdateUWeight,
				cellStateUpdateBias,
				outputGateWeight,
				outputGateUWeight,
				outputGateBias,
				cellState = 0,
				hiddenState = 0;
		public Func<double, double> 
			sigmoid = ActivationFunction.Sigmoid, 
			tanh = ActivationFunction.Tanh;

		public LongShortTermMemory(Func<double, double, double> lossFunction)
		{
			Random random = new Random();
			double RandomInitValue() => random.NextDouble() * 2 - 1;
			forgetGateWeight = RandomInitValue();
			forgetGateUWeight = RandomInitValue();
			forgetGateBias = RandomInitValue();
			inputGateWeight = RandomInitValue();
			inputGateUWeight = RandomInitValue();
			inputGateBias = RandomInitValue();
			cellStateUpdateWeight = RandomInitValue();
			cellStateUpdateUWeight = RandomInitValue();
			cellStateUpdateBias = RandomInitValue();
			outputGateWeight = RandomInitValue();
			outputGateUWeight = RandomInitValue();
			outputGateBias = RandomInitValue();
		}

		public double[] Predict(double[] inputs)
		{
			List<double> outputs = new List<double>();
			for (int i = 0; i < inputs.Length; i++)
			{
				double input = inputs[i];
				double forgetGate = sigmoid((input * forgetGateWeight) + (hiddenState * forgetGateUWeight) + forgetGateBias);
				cellState *= forgetGate;
				double inputGate = sigmoid((input * inputGateWeight) + (hiddenState * inputGateUWeight) + inputGateBias);
				cellState += inputGate * tanh((input * cellStateUpdateWeight) + (hiddenState * cellStateUpdateUWeight) + cellStateUpdateBias);
				double outputGate = sigmoid((input * outputGateWeight) + (hiddenState * outputGateUWeight) + outputGateBias);
				hiddenState = outputGate * tanh(cellState);
				outputs.Add(hiddenState);
			}
			return outputs.ToArray();
		}

		public void Train(double[] inputs, double[] targetOutputs)
		{

		}
	}
}
