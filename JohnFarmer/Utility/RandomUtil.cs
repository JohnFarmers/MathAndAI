using System;

namespace JohnFarmer.Utility
{
	public static class RandomUtil
	{
		private static readonly Random random = new Random();

		public static int Range(int minInclusive, int maxExclusive) => random.Next(minInclusive, maxExclusive);

		public static float Range(float minInclusive, float maxExclusive) => random.NextSingle() * (maxExclusive - minInclusive) + minInclusive;
		
		public static double Range(double minInclusive, double maxExclusive) => random.NextDouble() * (maxExclusive - minInclusive) + minInclusive;
	}
}
