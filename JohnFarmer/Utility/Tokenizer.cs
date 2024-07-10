using CommunityToolkit.HighPerformance;
using System;
using System.Collections.Generic;

namespace MathAndAI.JohnFarmer.Utility
{
	public static class Tokenizer
	{
		public static ReadOnlySpan<string> Tokenize(params string[] corpus)
		{
			List<string> tokens = new List<string>();
			ReadOnlySpan<string> corpusSpan = corpus.AsSpan();
			for (int i = 0; i < corpusSpan.Length; i++)
			{
				string[] words = corpusSpan[i].ToLower().Split(' ');
				for (int j = 0; j < words.Length; j++)
				{
					if (!tokens.Contains(words[j]))
						tokens.Add(words[j]);
				}
			}
			return tokens.AsSpan();
		}
	}
}
