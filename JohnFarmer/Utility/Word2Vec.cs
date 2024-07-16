using JohnFarmer.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using JohnFarmer.Utility;
using Plotly.NET;

namespace MathAndAI.JohnFarmer.Utility
{
	public class Word2Vec
	{
		public string[] corpus, tokens;
		public Variable weights1;
		public Variable weights2;
		private int contextWindow;

		public Word2Vec(string[] corpus, int dimension, int contextWindow = 2)
		{
			this.corpus = corpus;
			List<string> tokens = Tokenizer.Tokenize(corpus).ToArray().ToList();
			tokens.Add("<EOS>");
			this.tokens = tokens.ToArray();
			weights1 = new Variable(new Matrix(dimension, this.tokens.Length).Randomize(), true);
			weights2 = new Variable(new Matrix(this.tokens.Length, dimension).Randomize(), true);
			this.contextWindow = contextWindow;
		}

		public double[] Forward(string word)
		{
			if(tokens.Contains(word)) 
			{
				int index = Array.IndexOf(tokens, word);
				double[] inputs = new double[tokens.Length];
				inputs[index] = 1;
				return ((Matrix)(Matrix.MatMul(weights2.value, Matrix.MatMul(weights1.value, inputs.To1DMatrix())))).ToArray();
			}
			return null;
		}

		public void Train()
		{
			for (int i = 0; i < corpus.Length; i++)
			{
				ReadOnlySpan<string> wordTokens = Tokenizer.Tokenize(corpus[i]);
				for (int j = 0; j < wordTokens.Length; j++)
				{
					double[] inputs = new double[tokens.Length];
					double[] outputs = new double[tokens.Length];

					string inputToken = wordTokens[j];
					int inputIndex = Array.IndexOf(tokens, inputToken);
					if (inputIndex == -1)
						continue;
					inputs[inputIndex] = 1d;

					string targetToken = j + 1 < wordTokens.Length ? wordTokens[j + 1] : "<EOS>";
					int outputIndex = Array.IndexOf(tokens, targetToken);
					if (outputIndex == -1) 
						continue;
					outputs[outputIndex] = 1d;

					Operation op1 = Operation.MatMul(weights1, inputs.To1DMatrix());
					Operation op2 = Operation.MatMul(weights2, op1);
					Operation loss = Operation.CrossEntropyLoss(op2, outputs.To1DMatrix());
					loss.Backward();
					weights1.Optimize(.1d);
					weights2.Optimize(.1d);
				}
			}
		}

		public void ShowPlot()
		{
			List<double> x = new List<double>();
			List<double> y = new List<double>();

			for (int i = 0; i <weights1.value.columns; i++)
			{
				x.Add(weights1.value[0, i]);
				y.Add(weights1.value[1, i]);
			}

			var scatter = Chart2D.Chart.Point<double, double, string>(new double[] { }, new double[] { }, null);

			for (int i = 0; i < tokens.Length; i++)
			{
				scatter.WithAnnotations(new Plotly.NET.LayoutObjects.Annotation[]
				{
					Plotly.NET.LayoutObjects.Annotation.init<double, double, string, string, string, string, string, string, string, string>(
						X: x[i],
						Y: y[i],
						Text: tokens[i],
						ShowArrow: false
					)
				});
			}

			scatter.Show();
		}
	}
}
