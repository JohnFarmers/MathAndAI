using System;
using System.Collections.Generic;
using JohnFarmer.Math;

namespace JohnFarmer.NeuralNetwork
{
    public class NeuralNetwork
    {
        public readonly int[] layers;
        public List<Matrix> weights, biases;
        public Func<double, double> activationFunction = ActivationFunction.Sigmoid, activationFunctionDerivative = ActivationFunction.SigmoidPrime;
        public Func<Matrix, Matrix, Matrix> 
            costFunction = (Matrix x, Matrix y) => Matrix.Power(y - x, 2),
            costFunctionDerivative = (Matrix x, Matrix y) => (y - x) * -2;
        public double learningRate = .1;

        public NeuralNetwork(params int[] layers)
        {
            this.layers = layers;
            weights = new List<Matrix>();
            biases = new List<Matrix>();
			for (int i = 0; i < layers.Length - 1; i++)
            {
                weights.Add(new Matrix(layers[i + 1], layers[i]).Randomize());
                biases.Add(new Matrix(layers[i + 1], 1).Randomize());
            }
        }

		/// <summary>
		/// Perform a forward propagation and return the predicted value.
		/// </summary>
		public double[] Predict(double[] inputs)
        {
            Matrix prediction = new Matrix();
            Matrix matrixInputs = inputs.To1DMatrix();
            for (int i = 0; i < layers.Length - 1; i++)
            {
                prediction = (weights[i] * (i == 0 ? matrixInputs : prediction)) + biases[i];
				prediction.Map(activationFunction);
            }
            return prediction.ToArray();
        }

		/// <summary>
		/// Perform a forward propagatiom and train the neural network by performing a backpropagation.
		/// </summary>
		public void Train(double[] inputs, double[] targetOutputs)
        {
			List<Matrix> activations = new List<Matrix>(), weightedSums = new List<Matrix>();
			Matrix matrixInputs = inputs.To1DMatrix();
			for (int i = 0; i < layers.Length - 1; i++)
            {
                Matrix weightedSum = (weights[i] * (i == 0 ? matrixInputs : activations[i - 1])) + biases[i];
                weightedSums.Add(weightedSum);
				activations.Add(Matrix.Map(weightedSum, activationFunction));
            }
            Matrix outputs = activations[^1];
            List<Matrix> errors = new List<Matrix>();
            Matrix outputError = Matrix.HadamardProduct(costFunctionDerivative(outputs, targetOutputs.To1DMatrix()), Matrix.Map(outputs, activationFunctionDerivative));
            errors.Add(outputError);
            for (int l = layers.Length - 1; l > 0; l--)
            {
                Matrix weight = weights[l - 1].Transpose();
                Matrix error = Matrix.HadamardProduct(weight * errors[layers.Length - 1 - l], Matrix.Map(l - 2 < 0 ? matrixInputs : activations[l - 2], activationFunctionDerivative));
                errors.Add(error);
                weights[l - 1] -= error * learningRate;
            }
            for (int l = layers.Length; l > 0; l--)
            {
                
            }
        }
    }
}
