using System;
using System.Collections.Generic;

namespace JohnFarmer.Utility
{
	public static class ListExtension
	{
		public static T GetRandom<T>(this List<T> list) => list[RandomUtil.Range(0, list.Count)];
		
		public static T GetRandom<T>(this List<T> list, int minIndex, int maxIndex)
		{
			if (minIndex < 0 || maxIndex > list.Count - 1)
				throw new IndexOutOfRangeException();
			return list[RandomUtil.Range(minIndex, maxIndex + 1)];
		}
	}
}
