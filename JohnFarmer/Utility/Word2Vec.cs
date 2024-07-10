using JohnFarmer.NeuralNetwork.Matrices;
using JohnFarmer.NeuralNetwork;
using JohnFarmer.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JohnFarmer.Utility;

namespace MathAndAI.JohnFarmer.Utility
{
	public class Word2Vec
	{
		public string[] corpus, tokens;
		public Variable weights1;
		public Variable weights2;

		public Word2Vec(string[] corpus, int dimension)
		{
			this.corpus = corpus;
			tokens = Tokenizer.Tokenize(corpus).ToArray();
			weights1 = new(new Matrix(dimension, tokens.Length).Randomize(), true);
			weights2 = new(new Matrix(tokens.Length, dimension).Randomize(), true);
		}

		public double[] Forward(string word)
		{
			if(tokens.Contains(word)) 
			{
				int index = Array.IndexOf(tokens, word);
				double[] inputs = new double[tokens.Length];
				inputs[index] = 1;
				return ((Matrix)(weights2 * (weights1 * inputs.To1DMatrix()).result)).ToArray();
			}
			return null;
		}

		public void Train()
		{
			for (int i = 0; i < tokens.Length; i++)
			{
				double[] inputs = new double[tokens.Length];
				double[] outputs = new double[tokens.Length];
				string token = tokens[i];
				int index = Array.IndexOf(tokens, token);
				inputs[index] = 1d;
				for (int j = 0; j < tokens.Length; j++)
				{
					if (j == index)
					{
						outputs[j] = 0;
						continue;
					}
					outputs[j] = 1;
				}
				Operation op1 = Operation.MatMul(weights1, inputs.To1DMatrix());
				Operation op2 = Operation.Multiply(weights2, op1);
				Operation loss = Operation.CrossEntropyLoss(op2, outputs.To1DMatrix());
				loss.Backward();
				weights1.Optimize(.1d);
				weights2.Optimize(.1d);
			}
		}
	}
}
