using CommunityToolkit.HighPerformance;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MathAndAI.JohnFarmer.Utility
{
	public static partial class Tokenizer
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
					string word = RemoveSymbols(words[j]);
					if (!tokens.Contains(word))
						tokens.Add(word);
				}
			}
			return tokens.AsSpan();
		}

		public static ReadOnlySpan<string> Tokenize(string sentence)
		{
			List<string> tokens = new List<string>();
			ReadOnlySpan<string> tokenSpan = sentence.Split(' ').AsSpan();
			for (int i = 0; i < tokenSpan.Length; i++)
				tokens.Add(tokenSpan[i].ToLower());
			return tokens.AsSpan();
		}

		[GeneratedRegex("[^a-zA-Z0-9_.]+", RegexOptions.Compiled)]
		private static partial Regex Regex();

		private static string RemoveSymbols(string str) => Regex().Replace(str, "");
	}
}
