using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathAndAI.JohnFarmer.Utility
{
	public static class ListExtension
	{
		public static T GetRandom<T>(this List<T> list) => list[new Random().Next(0, list.Count)];
		
		public static T GetRandom<T>(this List<T> list, int minIndex, int maxIndex)
		{
			if (minIndex < 0 || maxIndex > list.Count - 1)
				throw new IndexOutOfRangeException();
			return list[new Random().Next(minIndex, maxIndex + 1)];
		}
	}
}
