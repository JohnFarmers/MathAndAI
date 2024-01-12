using JohnFarmer.Utility;
using System;

namespace JohnFarmer.NeuralNetwork
{
	public class LSTM
	{
		private Variable 
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
				outputGateBias;
		private double learningRate;
		private readonly Func<dynamic, dynamic> 
			sigmoid = ActivationFunction.Sigmoid, 
			tanh = ActivationFunction.Tanh;

		public LSTM(Func<dynamic, dynamic, dynamic> lossFunction, double learningRate)
		{
			double RandomInitValue() => RandomUtil.Range(-1d, 1.1d);
			forgetGateWeight = new(RandomInitValue(), true);
			forgetGateUWeight = new(RandomInitValue(), true);
			forgetGateBias = new(RandomInitValue(), true);
			inputGateWeight = new(RandomInitValue(), true);
			inputGateUWeight = new(RandomInitValue(), true);
			inputGateBias = new(RandomInitValue(), true);
			cellStateUpdateWeight = new(RandomInitValue(), true);
			cellStateUpdateUWeight = new(RandomInitValue(), true);
			cellStateUpdateBias = new(RandomInitValue(), true);
			outputGateWeight = new(RandomInitValue(), true);
			outputGateUWeight = new(RandomInitValue(), true);
			outputGateBias = new(RandomInitValue(), true);
			this.learningRate = learningRate;
		}

		public double Forward(double[] inputs)
		{
			if (inputs.Length == 0)
				throw new Exception("Input must have atleast 1 value.");
			double cellState = 0;
			double hiddenState = 0;
			double output = 0;
			for (int i = 0; i < inputs.Length; i++)
			{
				double input = inputs[i];
				double forgetGate = sigmoid((input * forgetGateWeight) + (hiddenState * forgetGateUWeight) + forgetGateBias);
				cellState *= forgetGate;
				double inputGate = sigmoid((input * inputGateWeight) + (hiddenState * inputGateUWeight) + inputGateBias);
				cellState += inputGate * tanh((input * cellStateUpdateWeight) + (hiddenState * cellStateUpdateUWeight) + cellStateUpdateBias);
				double outputGate = sigmoid((input * outputGateWeight) + (hiddenState * outputGateUWeight) + outputGateBias);
				hiddenState = outputGate * tanh(cellState);
				output = hiddenState;
			}
			return output;
		}

		public void Train(double[] inputs, double targetOutput)
		{
			double cellState = 0;
			double hiddenState = 0;
			Operation totalLoss = null;
			for (int i = 0; i < inputs.Length; i++)
			{
				double input = inputs[i];
				Operation forgetGate = Operation.Sigmoid((input * forgetGateWeight) + (hiddenState * forgetGateUWeight) + forgetGateBias);
				Operation newCellState1 = forgetGate * cellState;
				Operation inputGate = Operation.Sigmoid((input * inputGateWeight) + (hiddenState * inputGateUWeight) + inputGateBias);
				Operation newCellState2 = newCellState1 + (inputGate * Operation.Tanh((input * cellStateUpdateWeight) + (hiddenState * cellStateUpdateUWeight) + cellStateUpdateBias));
				cellState = newCellState2;
				Operation outputGate = Operation.Sigmoid((input * outputGateWeight) + (hiddenState * outputGateUWeight) + outputGateBias);
				Operation newHiddenState = outputGate * Operation.Tanh(newCellState2);
				hiddenState = newHiddenState;
				Operation loss = Operation.SquaredError(newHiddenState, targetOutput);
				totalLoss = totalLoss == null ? loss : Operation.Add(totalLoss, loss);
			}
			totalLoss.Backward();
			forgetGateWeight.Optimize(learningRate);
			forgetGateUWeight.Optimize(learningRate);
			forgetGateBias.Optimize(learningRate);
			inputGateWeight.Optimize(learningRate);
			inputGateUWeight.Optimize(learningRate);
			inputGateBias.Optimize(learningRate);
			cellStateUpdateWeight.Optimize(learningRate);
			cellStateUpdateUWeight.Optimize(learningRate);
			cellStateUpdateBias.Optimize(learningRate);
			outputGateWeight.Optimize(learningRate);
			outputGateUWeight.Optimize(learningRate);
			outputGateBias.Optimize(learningRate);
		}
	}
}
